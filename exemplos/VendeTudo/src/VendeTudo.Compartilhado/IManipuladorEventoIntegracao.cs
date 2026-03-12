namespace VendeTudo.Compartilhado;

/// <summary>
/// Interface para manipuladores de eventos de integração.
/// </summary>
/// <typeparam name="T">Tipo do evento de integração.</typeparam>
public interface IManipuladorEventoIntegracao<in T> where T : EventoIntegracaoBase
{
    /// <summary>
    /// Manipula o evento de integração recebido.
    /// </summary>
    /// <param name="evento">O evento a ser manipulado.</param>
    /// <returns>Task representando a operação assíncrona.</returns>
    Task ManipularAsync(T evento);
}
