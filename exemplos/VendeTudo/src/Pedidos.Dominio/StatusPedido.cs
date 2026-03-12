namespace VendeTudo.Pedidos.Dominio;

/// <summary>
/// Status possíveis de um pedido.
/// </summary>
public enum StatusPedido
{
    /// <summary>
    /// Pedido submetido, aguardando processamento.
    /// </summary>
    Submetido = 1,

    /// <summary>
    /// Pedido aguardando validação de estoque.
    /// </summary>
    AguardandoValidacao = 2,

    /// <summary>
    /// Estoque do pedido confirmado.
    /// </summary>
    EstoqueConfirmado = 3,

    /// <summary>
    /// Pagamento do pedido confirmado.
    /// </summary>
    Pago = 4,

    /// <summary>
    /// Pedido enviado para entrega.
    /// </summary>
    Enviado = 5,

    /// <summary>
    /// Pedido cancelado.
    /// </summary>
    Cancelado = 6
}
