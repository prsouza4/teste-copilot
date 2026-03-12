namespace VendeTudo.Compartilhado;

/// <summary>
/// Evento publicado quando um pedido é pago.
/// </summary>
public class PedidoPagoEventoIntegracao : EventoIntegracaoBase
{
    /// <summary>
    /// Identificador do pedido.
    /// </summary>
    public Guid IdPedido { get; }

    public PedidoPagoEventoIntegracao(Guid idPedido)
    {
        IdPedido = idPedido;
    }
}
