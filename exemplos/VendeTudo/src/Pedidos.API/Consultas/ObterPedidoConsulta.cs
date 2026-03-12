using MediatR;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.API.Consultas;

/// <summary>
/// Consulta para obter um pedido por ID.
/// </summary>
public record ObterPedidoConsulta(Guid IdPedido) : IRequest<PedidoDto?>;

/// <summary>
/// Handler para a consulta de pedido por ID.
/// </summary>
public class ObterPedidoHandler : IRequestHandler<ObterPedidoConsulta, PedidoDto?>
{
    private readonly IPedidoRepositorio _repositorio;

    public ObterPedidoHandler(IPedidoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<PedidoDto?> Handle(ObterPedidoConsulta request, CancellationToken cancellationToken)
    {
        var pedido = await _repositorio.ObterPorIdAsync(request.IdPedido, cancellationToken);
        if (pedido is null)
        {
            return null;
        }

        return new PedidoDto(
            pedido.Id,
            pedido.IdComprador,
            pedido.Status.ToString(),
            pedido.DataCriacao,
            pedido.Total,
            new EnderecoDto(
                pedido.Endereco.Rua,
                pedido.Endereco.Cidade,
                pedido.Endereco.Estado,
                pedido.Endereco.Pais,
                pedido.Endereco.Cep),
            pedido.Itens.Select(i => new ItemPedidoResponseDto(
                i.IdProduto,
                i.NomeProduto,
                i.PrecoUnitario,
                i.Unidades,
                i.UrlImagem
            ))
        );
    }
}
