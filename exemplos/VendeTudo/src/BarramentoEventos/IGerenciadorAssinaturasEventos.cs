using VendeTudo.Compartilhado;

namespace VendeTudo.BarramentoEventos;

/// <summary>
/// Interface para gerenciar assinaturas de eventos de integração.
/// </summary>
public interface IGerenciadorAssinaturasEventos
{
    /// <summary>
    /// Indica se não há assinaturas.
    /// </summary>
    bool EstaVazio { get; }

    /// <summary>
    /// Evento disparado quando um evento é removido.
    /// </summary>
    event EventHandler<string> AoRemoverEvento;

    /// <summary>
    /// Adiciona uma assinatura para um tipo de evento.
    /// </summary>
    void AdicionarAssinatura<T, TM>()
        where T : EventoIntegracaoBase
        where TM : IManipuladorEventoIntegracao<T>;

    /// <summary>
    /// Remove uma assinatura para um tipo de evento.
    /// </summary>
    void RemoverAssinatura<T, TM>()
        where T : EventoIntegracaoBase
        where TM : IManipuladorEventoIntegracao<T>;

    /// <summary>
    /// Verifica se há assinaturas para um tipo de evento.
    /// </summary>
    bool TemAssinaturasParaEvento<T>() where T : EventoIntegracaoBase;

    /// <summary>
    /// Verifica se há assinaturas para um nome de evento.
    /// </summary>
    bool TemAssinaturasParaEvento(string nomeEvento);

    /// <summary>
    /// Obtém o tipo do evento pelo nome.
    /// </summary>
    Type? ObterTipoEventoPorNome(string nomeEvento);

    /// <summary>
    /// Limpa todas as assinaturas.
    /// </summary>
    void Limpar();

    /// <summary>
    /// Obtém todas as assinaturas para um nome de evento.
    /// </summary>
    IEnumerable<InformacaoAssinatura> ObterManipuladoresParaEvento(string nomeEvento);

    /// <summary>
    /// Obtém o nome do evento a partir do tipo.
    /// </summary>
    string ObterNomeEvento<T>();
}
