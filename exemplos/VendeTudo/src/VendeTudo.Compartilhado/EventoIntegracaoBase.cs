namespace VendeTudo.Compartilhado;

/// <summary>
/// Classe base para eventos de integração entre serviços.
/// </summary>
public abstract class EventoIntegracaoBase
{
    /// <summary>
    /// Identificador único do evento.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Data e hora de criação do evento.
    /// </summary>
    public DateTime DataHoraCriacao { get; }

    protected EventoIntegracaoBase()
    {
        Id = Guid.NewGuid();
        DataHoraCriacao = DateTime.UtcNow;
    }

    protected EventoIntegracaoBase(Guid id, DateTime dataHoraCriacao)
    {
        Id = id;
        DataHoraCriacao = dataHoraCriacao;
    }
}
