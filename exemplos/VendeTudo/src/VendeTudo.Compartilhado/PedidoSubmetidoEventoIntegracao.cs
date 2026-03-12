namespace VendeTudo.Compartilhado;

/// <summary>
/// Evento publicado quando um pedido é submetido.
/// </summary>
public class PedidoSubmetidoEventoIntegracao : EventoIntegracaoBase
{
    /// <summary>
    /// Identificador do pedido.
    /// </summary>
    public Guid IdPedido { get; }

    /// <summary>
    /// Identificador do comprador.
    /// </summary>
    public string IdComprador { get; }

    public PedidoSubmetidoEventoIntegracao(Guid idPedido, string idComprador)
    {
        IdPedido = idPedido;
        IdComprador = idComprador;
    }
}
