using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VendeTudo.BarramentoEventos;
using VendeTudo.Compartilhado;

namespace VendeTudo.BarramentoEventosRabbitMQ;

/// <summary>
/// Implementação do barramento de eventos usando RabbitMQ.
/// </summary>
public class BarramentoEventosRabbitMQ : IBarramentoEventos, IDisposable
{
    private const string NomeExchange = "vendetudo_event_bus";

    private readonly ConfiguracaoRabbitMQ _configuracao;
    private readonly IGerenciadorAssinaturasEventos _gerenciadorAssinaturas;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BarramentoEventosRabbitMQ> _logger;

    private IConnection? _conexao;
    private IChannel? _canalConsumidor;
    private bool _disposed;

    public BarramentoEventosRabbitMQ(
        IOptions<ConfiguracaoRabbitMQ> configuracao,
        IGerenciadorAssinaturasEventos gerenciadorAssinaturas,
        IServiceProvider serviceProvider,
        ILogger<BarramentoEventosRabbitMQ> logger)
    {
        _configuracao = configuracao.Value;
        _gerenciadorAssinaturas = gerenciadorAssinaturas;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void Publicar<T>(T evento) where T : EventoIntegracaoBase
    {
        var nomeEvento = evento.GetType().Name;
        var mensagem = JsonSerializer.Serialize(evento, evento.GetType());
        var corpo = Encoding.UTF8.GetBytes(mensagem);

        var canal = CriarCanal();
        // RabbitMQ.Client v7 usa API async; GetAwaiter().GetResult() é seguro em ASP.NET Core
        // pois não existe SynchronizationContext, evitando deadlocks.
        canal.ExchangeDeclareAsync(NomeExchange, ExchangeType.Direct).GetAwaiter().GetResult();

        var propriedades = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
            ContentType = "application/json"
        };

        canal.BasicPublishAsync(NomeExchange, nomeEvento, false, propriedades, corpo).GetAwaiter().GetResult();

        _logger.LogInformation("Evento {NomeEvento} publicado com ID {IdEvento}", nomeEvento, evento.Id);
    }

    public void Assinar<T, TM>()
        where T : EventoIntegracaoBase
        where TM : IManipuladorEventoIntegracao<T>
    {
        var nomeEvento = _gerenciadorAssinaturas.ObterNomeEvento<T>();
        _gerenciadorAssinaturas.AdicionarAssinatura<T, TM>();

        IniciarConsumoBasico();

        var canal = _canalConsumidor;
        if (canal is null)
        {
            return;
        }

        canal.QueueBindAsync(_configuracao.NomeFilaAssinatura, NomeExchange, nomeEvento).GetAwaiter().GetResult();

        _logger.LogInformation("Assinatura registrada para {NomeEvento} com manipulador {TipoManipulador}",
            nomeEvento, typeof(TM).Name);
    }

    public void CancelarAssinatura<T, TM>()
        where T : EventoIntegracaoBase
        where TM : IManipuladorEventoIntegracao<T>
    {
        _gerenciadorAssinaturas.RemoverAssinatura<T, TM>();
        _logger.LogInformation("Assinatura cancelada para {TipoEvento}", typeof(T).Name);
    }

    private IChannel CriarCanal()
    {
        if (_conexao is null || !_conexao.IsOpen)
        {
            CriarConexao();
        }

        return _conexao!.CreateChannelAsync().GetAwaiter().GetResult();
    }

    private void CriarConexao()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuracao.ServidorRabbitMQ,
            Port = _configuracao.Porta,
            UserName = _configuracao.Usuario,
            Password = _configuracao.Senha
        };

        var tentativas = 0;
        while (tentativas < _configuracao.TentativasReconexao)
        {
            try
            {
                // CreateConnectionAsync é seguro com GetAwaiter().GetResult() em ASP.NET Core
                // (sem SynchronizationContext = sem risco de deadlock)
                _conexao = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                _logger.LogInformation("Conexão RabbitMQ estabelecida com {Servidor}:{Porta}",
                    _configuracao.ServidorRabbitMQ, _configuracao.Porta);
                return;
            }
            catch (Exception ex)
            {
                tentativas++;
                _logger.LogWarning(ex, "Falha ao conectar ao RabbitMQ. Tentativa {Tentativa}/{Total}",
                    tentativas, _configuracao.TentativasReconexao);

                if (tentativas >= _configuracao.TentativasReconexao)
                {
                    throw;
                }

                Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, tentativas))).GetAwaiter().GetResult();
            }
        }
    }

    private void IniciarConsumoBasico()
    {
        if (_canalConsumidor is not null)
        {
            return;
        }

        _canalConsumidor = CriarCanal();

        _canalConsumidor.ExchangeDeclareAsync(NomeExchange, ExchangeType.Direct).GetAwaiter().GetResult();
        _canalConsumidor.QueueDeclareAsync(_configuracao.NomeFilaAssinatura, true, false, false).GetAwaiter().GetResult();

        var consumidor = new AsyncEventingBasicConsumer(_canalConsumidor);
        consumidor.ReceivedAsync += async (model, ea) =>
        {
            var nomeEvento = ea.RoutingKey;
            var mensagem = Encoding.UTF8.GetString(ea.Body.ToArray());

            try
            {
                await ProcessarEvento(nomeEvento, mensagem);
                await _canalConsumidor.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar evento {NomeEvento}", nomeEvento);
                await _canalConsumidor.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        _canalConsumidor.BasicConsumeAsync(_configuracao.NomeFilaAssinatura, false, consumidor).GetAwaiter().GetResult();
    }

    private async Task ProcessarEvento(string nomeEvento, string mensagem)
    {
        _logger.LogDebug("Processando evento {NomeEvento}", nomeEvento);

        if (!_gerenciadorAssinaturas.TemAssinaturasParaEvento(nomeEvento))
        {
            _logger.LogWarning("Nenhuma assinatura encontrada para evento {NomeEvento}", nomeEvento);
            return;
        }

        var assinaturas = _gerenciadorAssinaturas.ObterManipuladoresParaEvento(nomeEvento);
        var tipoEvento = _gerenciadorAssinaturas.ObterTipoEventoPorNome(nomeEvento);

        if (tipoEvento is null)
        {
            return;
        }

        var evento = JsonSerializer.Deserialize(mensagem, tipoEvento);

        foreach (var assinatura in assinaturas)
        {
            using var scope = _serviceProvider.CreateScope();
            var manipulador = scope.ServiceProvider.GetService(assinatura.TipoManipulador);

            if (manipulador is null)
            {
                _logger.LogWarning("Manipulador {TipoManipulador} não encontrado", assinatura.TipoManipulador.Name);
                continue;
            }

            var tipoConcreto = typeof(IManipuladorEventoIntegracao<>).MakeGenericType(tipoEvento);
            var metodo = tipoConcreto.GetMethod("ManipularAsync");

            if (metodo is not null && evento is not null)
            {
                await (Task)metodo.Invoke(manipulador, [evento])!;
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        _canalConsumidor?.CloseAsync().GetAwaiter().GetResult();
        _canalConsumidor?.Dispose();

        _conexao?.CloseAsync().GetAwaiter().GetResult();
        _conexao?.Dispose();

        GC.SuppressFinalize(this);
    }
}
