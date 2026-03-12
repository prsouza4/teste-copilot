using Microsoft.EntityFrameworkCore;
using VendeTudo.BarramentoEventos;
using VendeTudo.BarramentoEventosRabbitMQ;
using VendeTudo.Catalogo.API;
using VendeTudo.PadroeServico;

var builder = WebApplication.CreateBuilder(args);

// Padrões de serviço
builder.AdicionarPadroesServico();

// Database
builder.Services.AddDbContext<CatalogoDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("BancoCatalogo"));
});

// Barramento de eventos
builder.Services.AdicionarBarramentoEventosRabbitMQ(config =>
{
    var section = builder.Configuration.GetSection("BarramentoEventos");
    config.ServidorRabbitMQ = section["ServidorRabbitMQ"] ?? "localhost";
    config.Porta = int.Parse(section["Porta"] ?? "5672");
    config.Usuario = section["Usuario"] ?? "guest";
    config.Senha = section["Senha"] ?? "guest";
    config.NomeFilaAssinatura = section["NomeFilaAssinatura"] ?? "catalogo_api_queue";
});

// Semeador de dados
builder.Services.AddHostedService<SemeadorDadosCatalogo>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapearEndpointsCatalogo();
app.MapearEndpointsPadrao();

app.Run();

public partial class Program { }
