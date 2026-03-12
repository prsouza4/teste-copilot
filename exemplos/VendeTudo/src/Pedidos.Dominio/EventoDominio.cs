namespace VendeTudo.Pedidos.Dominio;

/// <summary>
/// Classe base para eventos de domínio.
/// </summary>
public abstract class EventoDominio
{
    /// <summary>
    /// Data e hora de ocorrência do evento.
    /// </summary>
    public DateTime DataHoraOcorrencia { get; }

    protected EventoDominio()
    {
        DataHoraOcorrencia = DateTime.UtcNow;
    }
}
