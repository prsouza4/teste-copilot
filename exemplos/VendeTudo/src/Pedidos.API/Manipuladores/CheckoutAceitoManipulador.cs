using VendeTudo.BarramentoEventos;
using VendeTudo.Compartilhado;
using VendeTudo.Pedidos.API.Comandos;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.API.Manipuladores;

/// <summary>
/// Manipulador do evento de checkout aceito.
/// </summary>
public class CheckoutAceitoManipulador : IManipuladorEventoIntegracao<CheckoutAceitoEventoIntegracao>
{
    private readonly IPedidoRepositorio _repositorio;
    private readonly IBarramentoEventos _barramento;
    private readonly ILogger<CheckoutAceitoManipulador> _logger;

    public CheckoutAceitoManipulador(
        IPedidoRepositorio repositorio,
        IBarramentoEventos barramento,
        ILogger<CheckoutAceitoManipulador> logger)
    {
        _repositorio = repositorio;
        _barramento = barramento;
        _logger = logger;
    }

    public async Task ManipularAsync(CheckoutAceitoEventoIntegracao evento)
    {
        _logger.LogInformation("Processando checkout aceito para comprador {IdComprador}",
            evento.IdComprador);

        var endereco = EnderecoEntrega.Criar(
            evento.Endereco.Rua,
            evento.Endereco.Cidade,
            evento.Endereco.Estado,
            evento.Endereco.Pais,
            evento.Endereco.Cep);

        var pedido = PedidoAgregado.CriarPedido(evento.IdComprador, endereco);

        foreach (var item in evento.Itens)
        {
            pedido.AdicionarItemPedido(
                item.IdProduto,
                item.NomeProduto,
                item.PrecoUnitario,
                item.UrlImagem,
                item.Quantidade);
        }

        await _repositorio.AdicionarAsync(pedido);

        // Publica evento de pedido submetido
        var eventoSubmetido = new PedidoSubmetidoEventoIntegracao(pedido.Id, evento.IdComprador);
        _barramento.Publicar(eventoSubmetido);

        _logger.LogInformation("Pedido {IdPedido} criado a partir do checkout", pedido.Id);
    }
}
