using Microsoft.EntityFrameworkCore;
using VendeTudo.BarramentoEventos;
using VendeTudo.BarramentoEventosRabbitMQ;
using VendeTudo.Compartilhado;
using VendeTudo.PadroeServico;
using VendeTudo.Pedidos.API;
using VendeTudo.Pedidos.API.Manipuladores;
using VendeTudo.Pedidos.Dominio;
using VendeTudo.Pedidos.Infraestrutura;

var builder = WebApplication.CreateBuilder(args);

// Padrões de serviço
builder.AdicionarPadroesServico();

// Database
builder.Services.AddDbContext<PedidosDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("BancoPedidos"));
});

// Repositórios
builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

// Barramento de eventos
builder.Services.AdicionarBarramentoEventosRabbitMQ(config =>
{
    var section = builder.Configuration.GetSection("BarramentoEventos");
    config.ServidorRabbitMQ = section["ServidorRabbitMQ"] ?? "localhost";
    config.Porta = int.Parse(section["Porta"] ?? "5672");
    config.Usuario = section["Usuario"] ?? "guest";
    config.Senha = section["Senha"] ?? "guest";
    config.NomeFilaAssinatura = section["NomeFilaAssinatura"] ?? "pedidos_api_queue";
});

// Manipuladores de eventos
builder.Services.AddScoped<CheckoutAceitoManipulador>();

var app = builder.Build();

// Aplicar migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PedidosDbContext>();
    // EnsureCreatedAsync é intencional nesta aplicação de referência.
    // Em produção, substitua por context.Database.MigrateAsync() para gerenciar migrations.
    await context.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Assinar eventos
var barramento = app.Services.GetRequiredService<IBarramentoEventos>();
barramento.Assinar<CheckoutAceitoEventoIntegracao, CheckoutAceitoManipulador>();

app.MapearEndpointsPedidos();
app.MapearEndpointsPadrao();

app.Run();

public partial class Program { }
