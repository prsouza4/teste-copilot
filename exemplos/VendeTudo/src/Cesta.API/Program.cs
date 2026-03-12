using StackExchange.Redis;
using VendeTudo.BarramentoEventos;
using VendeTudo.BarramentoEventosRabbitMQ;
using VendeTudo.Cesta.API;
using VendeTudo.Compartilhado;
using VendeTudo.PadroeServico;

var builder = WebApplication.CreateBuilder(args);

// Padrões de serviço
builder.AdicionarPadroesServico();

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddScoped<ICestaRepositorio, CestaRepositorio>();

// Barramento de eventos
builder.Services.AdicionarBarramentoEventosRabbitMQ(config =>
{
    var section = builder.Configuration.GetSection("BarramentoEventos");
    config.ServidorRabbitMQ = section["ServidorRabbitMQ"] ?? "localhost";
    config.Porta = int.Parse(section["Porta"] ?? "5672");
    config.Usuario = section["Usuario"] ?? "guest";
    config.Senha = section["Senha"] ?? "guest";
    config.NomeFilaAssinatura = section["NomeFilaAssinatura"] ?? "cesta_api_queue";
});

// Manipuladores de eventos
builder.Services.AddScoped<PrecoItemAlteradoManipulador>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Assinar eventos
var barramento = app.Services.GetRequiredService<IBarramentoEventos>();
barramento.Assinar<PrecoItemAlteradoEventoIntegracao, PrecoItemAlteradoManipulador>();

app.MapearEndpointsCesta();
app.MapearEndpointsPadrao();

app.Run();

public partial class Program { }
