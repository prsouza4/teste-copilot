using VendeTudo.BarramentoEventos;
using VendeTudo.Compartilhado;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.ProcessadorPedidos;

/// <summary>
/// Manipulador do evento de pedido submetido.
/// Avança o status do pedido para AguardandoValidacao e depois para EstoqueConfirmado.
/// </summary>
public class PedidoSubmetidoManipulador : IManipuladorEventoIntegracao<PedidoSubmetidoEventoIntegracao>
{
    private readonly IPedidoRepositorio _repositorio;
    private readonly IBarramentoEventos _barramento;
    private readonly ILogger<PedidoSubmetidoManipulador> _logger;

    public PedidoSubmetidoManipulador(
        IPedidoRepositorio repositorio,
        IBarramentoEventos barramento,
        ILogger<PedidoSubmetidoManipulador> logger)
    {
        _repositorio = repositorio;
        _barramento = barramento;
        _logger = logger;
    }

    public async Task ManipularAsync(PedidoSubmetidoEventoIntegracao evento)
    {
        _logger.LogInformation("Processando pedido submetido {IdPedido}", evento.IdPedido);

        var pedido = await _repositorio.ObterPorIdAsync(evento.IdPedido);
        if (pedido is null)
        {
            _logger.LogWarning("Pedido {IdPedido} não encontrado", evento.IdPedido);
            return;
        }

        // Simula validação
        await Task.Delay(500);
        pedido.DefinirStatusAguardandoValidacao();
        await _repositorio.AtualizarAsync(pedido);
        _logger.LogInformation("Pedido {IdPedido} aguardando validação", evento.IdPedido);

        // Simula confirmação de estoque
        await Task.Delay(500);
        pedido.DefinirStatusEstoqueConfirmado();
        await _repositorio.AtualizarAsync(pedido);
        _logger.LogInformation("Estoque confirmado para pedido {IdPedido}", evento.IdPedido);

        // Publica evento de estoque confirmado
        var eventoEstoque = new EstoqueConfirmadoEventoIntegracao(pedido.Id);
        _barramento.Publicar(eventoEstoque);
    }
}
