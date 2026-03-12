using Microsoft.Extensions.DependencyInjection;
using VendeTudo.BarramentoEventos;

namespace VendeTudo.BarramentoEventosRabbitMQ;

/// <summary>
/// Extensões para configuração do barramento de eventos RabbitMQ.
/// </summary>
public static class ExtensoesBarramentoEventosRabbitMQ
{
    /// <summary>
    /// Adiciona o barramento de eventos RabbitMQ aos serviços.
    /// </summary>
    public static IServiceCollection AdicionarBarramentoEventosRabbitMQ(
        this IServiceCollection services,
        Action<ConfiguracaoRabbitMQ> configuracao)
    {
        services.Configure(configuracao);
        services.AddSingleton<IGerenciadorAssinaturasEventos, GerenciadorAssinaturasEventosEmMemoria>();
        services.AddSingleton<IBarramentoEventos, BarramentoEventosRabbitMQ>();

        return services;
    }
}
