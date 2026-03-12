namespace VendeTudo.Compartilhado;

/// <summary>
/// Evento publicado quando o estoque de um pedido é confirmado.
/// </summary>
public class EstoqueConfirmadoEventoIntegracao : EventoIntegracaoBase
{
    /// <summary>
    /// Identificador do pedido.
    /// </summary>
    public Guid IdPedido { get; }

    public EstoqueConfirmadoEventoIntegracao(Guid idPedido)
    {
        IdPedido = idPedido;
    }
}
