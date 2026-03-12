using MediatR;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.API.Consultas;

/// <summary>
/// Consulta para obter todos os pedidos.
/// </summary>
public record ObterPedidosConsulta : IRequest<IEnumerable<PedidoDto>>;

/// <summary>
/// DTO para retorno de pedido.
/// </summary>
public record PedidoDto(
    Guid Id,
    string IdComprador,
    string Status,
    DateTime DataCriacao,
    decimal Total,
    EnderecoDto Endereco,
    IEnumerable<ItemPedidoResponseDto> Itens
);

/// <summary>
/// DTO para endereço no retorno.
/// </summary>
public record EnderecoDto(string Rua, string Cidade, string Estado, string Pais, string Cep);

/// <summary>
/// DTO para item de pedido no retorno.
/// </summary>
public record ItemPedidoResponseDto(
    int IdProduto,
    string NomeProduto,
    decimal PrecoUnitario,
    int Quantidade,
    string? UrlImagem
);

/// <summary>
/// Handler para a consulta de pedidos.
/// </summary>
public class ObterPedidosHandler : IRequestHandler<ObterPedidosConsulta, IEnumerable<PedidoDto>>
{
    private readonly IPedidoRepositorio _repositorio;

    public ObterPedidosHandler(IPedidoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<PedidoDto>> Handle(
        ObterPedidosConsulta request,
        CancellationToken cancellationToken)
    {
        var pedidos = await _repositorio.ObterTodosAsync(cancellationToken);

        return pedidos.Select(p => new PedidoDto(
            p.Id,
            p.IdComprador,
            p.Status.ToString(),
            p.DataCriacao,
            p.Total,
            new EnderecoDto(p.Endereco.Rua, p.Endereco.Cidade, p.Endereco.Estado, p.Endereco.Pais, p.Endereco.Cep),
            p.Itens.Select(i => new ItemPedidoResponseDto(
                i.IdProduto,
                i.NomeProduto,
                i.PrecoUnitario,
                i.Unidades,
                i.UrlImagem
            ))
        ));
    }
}
