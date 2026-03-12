using Microsoft.EntityFrameworkCore;
using VendeTudo.BarramentoEventos;
using VendeTudo.BarramentoEventosRabbitMQ;
using VendeTudo.Compartilhado;
using VendeTudo.Pedidos.Dominio;
using VendeTudo.Pedidos.Infraestrutura;
using VendeTudo.ProcessadorPedidos;

var builder = Host.CreateApplicationBuilder(args);

// Database
builder.Services.AddDbContext<PedidosDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("BancoPedidos"));
});

// Repositórios
builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();

// Barramento de eventos
builder.Services.AdicionarBarramentoEventosRabbitMQ(config =>
{
    var section = builder.Configuration.GetSection("BarramentoEventos");
    config.ServidorRabbitMQ = section["ServidorRabbitMQ"] ?? "localhost";
    config.Porta = int.Parse(section["Porta"] ?? "5672");
    config.Usuario = section["Usuario"] ?? "guest";
    config.Senha = section["Senha"] ?? "guest";
    config.NomeFilaAssinatura = section["NomeFilaAssinatura"] ?? "processador_pedidos_queue";
});

// Manipuladores
builder.Services.AddScoped<PedidoSubmetidoManipulador>();

var host = builder.Build();

// Assinar eventos
var barramento = host.Services.GetRequiredService<IBarramentoEventos>();
barramento.Assinar<PedidoSubmetidoEventoIntegracao, PedidoSubmetidoManipulador>();

host.Run();
