using VendeTudo.BarramentoEventos;
using VendeTudo.Compartilhado;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.ProcessadorPagamentos;

/// <summary>
/// Manipulador do evento de estoque confirmado.
/// Avança o status do pedido para Pago.
/// </summary>
public class EstoqueConfirmadoManipulador : IManipuladorEventoIntegracao<EstoqueConfirmadoEventoIntegracao>
{
    private readonly IPedidoRepositorio _repositorio;
    private readonly IBarramentoEventos _barramento;
    private readonly ILogger<EstoqueConfirmadoManipulador> _logger;

    public EstoqueConfirmadoManipulador(
        IPedidoRepositorio repositorio,
        IBarramentoEventos barramento,
        ILogger<EstoqueConfirmadoManipulador> logger)
    {
        _repositorio = repositorio;
        _barramento = barramento;
        _logger = logger;
    }

    public async Task ManipularAsync(EstoqueConfirmadoEventoIntegracao evento)
    {
        _logger.LogInformation("Processando estoque confirmado para pedido {IdPedido}", evento.IdPedido);

        var pedido = await _repositorio.ObterPorIdAsync(evento.IdPedido);
        if (pedido is null)
        {
            _logger.LogWarning("Pedido {IdPedido} não encontrado", evento.IdPedido);
            return;
        }

        // Simula processamento de pagamento
        await Task.Delay(1000);

        pedido.DefinirStatusPagoPendente();
        await _repositorio.AtualizarAsync(pedido);
        _logger.LogInformation("Pedido {IdPedido} marcado como pago", evento.IdPedido);

        // Publica evento de pedido pago
        var eventoPago = new PedidoPagoEventoIntegracao(pedido.Id);
        _barramento.Publicar(eventoPago);
    }
}
