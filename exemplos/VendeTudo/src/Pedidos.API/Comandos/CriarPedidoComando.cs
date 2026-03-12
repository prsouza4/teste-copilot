using MediatR;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.API.Comandos;

/// <summary>
/// Comando para criar um novo pedido.
/// </summary>
public record CriarPedidoComando(
    string IdComprador,
    string Rua,
    string Cidade,
    string Estado,
    string Pais,
    string Cep,
    List<ItemPedidoDto> Itens
) : IRequest<Guid>;

/// <summary>
/// DTO para um item de pedido.
/// </summary>
public record ItemPedidoDto(
    int IdProduto,
    string NomeProduto,
    decimal PrecoUnitario,
    string UrlImagem,
    int Quantidade
);

/// <summary>
/// Handler para o comando de criar pedido.
/// </summary>
public class CriarPedidoHandler : IRequestHandler<CriarPedidoComando, Guid>
{
    private readonly IPedidoRepositorio _repositorio;
    private readonly ILogger<CriarPedidoHandler> _logger;

    public CriarPedidoHandler(IPedidoRepositorio repositorio, ILogger<CriarPedidoHandler> logger)
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task<Guid> Handle(CriarPedidoComando request, CancellationToken cancellationToken)
    {
        var endereco = EnderecoEntrega.Criar(
            request.Rua,
            request.Cidade,
            request.Estado,
            request.Pais,
            request.Cep);

        var pedido = PedidoAgregado.CriarPedido(request.IdComprador, endereco);

        foreach (var item in request.Itens)
        {
            pedido.AdicionarItemPedido(
                item.IdProduto,
                item.NomeProduto,
                item.PrecoUnitario,
                item.UrlImagem,
                item.Quantidade);
        }

        await _repositorio.AdicionarAsync(pedido, cancellationToken);

        _logger.LogInformation("Pedido {IdPedido} criado para comprador {IdComprador}",
            pedido.Id, request.IdComprador);

        return pedido.Id;
    }
}
