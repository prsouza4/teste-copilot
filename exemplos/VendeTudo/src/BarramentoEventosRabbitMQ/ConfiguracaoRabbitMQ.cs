namespace VendeTudo.BarramentoEventosRabbitMQ;

/// <summary>
/// Configuração do RabbitMQ para o barramento de eventos.
/// </summary>
public class ConfiguracaoRabbitMQ
{
    /// <summary>
    /// Endereço do servidor RabbitMQ.
    /// </summary>
    public string ServidorRabbitMQ { get; set; } = "localhost";

    /// <summary>
    /// Porta do servidor RabbitMQ.
    /// </summary>
    public int Porta { get; set; } = 5672;

    /// <summary>
    /// Nome de usuário para conexão.
    /// </summary>
    public string Usuario { get; set; } = "guest";

    /// <summary>
    /// Senha para conexão.
    /// </summary>
    public string Senha { get; set; } = "guest";

    /// <summary>
    /// Nome da fila de assinatura para este serviço.
    /// </summary>
    public string NomeFilaAssinatura { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade de tentativas de reconexão.
    /// </summary>
    public int TentativasReconexao { get; set; } = 5;
}
