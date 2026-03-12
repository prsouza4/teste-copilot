using VendeTudo.Compartilhado;

namespace VendeTudo.BarramentoEventos;

/// <summary>
/// Interface do barramento de eventos para publicação e assinatura de eventos de integração.
/// </summary>
public interface IBarramentoEventos
{
    /// <summary>
    /// Publica um evento de integração.
    /// </summary>
    /// <typeparam name="T">Tipo do evento.</typeparam>
    /// <param name="evento">O evento a ser publicado.</param>
    void Publicar<T>(T evento) where T : EventoIntegracaoBase;

    /// <summary>
    /// Assina um tipo de evento de integração.
    /// </summary>
    /// <typeparam name="T">Tipo do evento.</typeparam>
    /// <typeparam name="TM">Tipo do manipulador do evento.</typeparam>
    void Assinar<T, TM>()
        where T : EventoIntegracaoBase
        where TM : IManipuladorEventoIntegracao<T>;

    /// <summary>
    /// Cancela a assinatura de um tipo de evento de integração.
    /// </summary>
    /// <typeparam name="T">Tipo do evento.</typeparam>
    /// <typeparam name="TM">Tipo do manipulador do evento.</typeparam>
    void CancelarAssinatura<T, TM>()
        where T : EventoIntegracaoBase
        where TM : IManipuladorEventoIntegracao<T>;
}
