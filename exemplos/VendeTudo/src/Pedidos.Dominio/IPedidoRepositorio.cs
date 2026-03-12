namespace VendeTudo.Pedidos.Dominio;

/// <summary>
/// Interface do repositório de pedidos.
/// </summary>
public interface IPedidoRepositorio
{
    /// <summary>
    /// Obtém um pedido pelo ID.
    /// </summary>
    Task<PedidoAgregado?> ObterPorIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Obtém todos os pedidos de um comprador.
    /// </summary>
    Task<IEnumerable<PedidoAgregado>> ObterPorCompradorAsync(string idComprador, CancellationToken ct = default);

    /// <summary>
    /// Adiciona um novo pedido.
    /// </summary>
    Task AdicionarAsync(PedidoAgregado pedido, CancellationToken ct = default);

    /// <summary>
    /// Atualiza um pedido existente.
    /// </summary>
    Task AtualizarAsync(PedidoAgregado pedido, CancellationToken ct = default);

    /// <summary>
    /// Remove um pedido.
    /// </summary>
    Task RemoverAsync(PedidoAgregado pedido, CancellationToken ct = default);

    /// <summary>
    /// Obtém todos os pedidos.
    /// </summary>
    Task<IEnumerable<PedidoAgregado>> ObterTodosAsync(CancellationToken ct = default);
}
