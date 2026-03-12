using MediatR;
using VendeTudo.Pedidos.API.Comandos;
using VendeTudo.Pedidos.API.Consultas;

namespace VendeTudo.Pedidos.API;

/// <summary>
/// Endpoints da API de Pedidos.
/// </summary>
public static class EndpointsPedidos
{
    public static void MapearEndpointsPedidos(this WebApplication app)
    {
        var grupo = app.MapGroup("/api/pedidos");

        grupo.MapGet("/", ObterPedidos);
        grupo.MapGet("/{id:guid}", ObterPedidoPorId);
        grupo.MapPost("/", CriarPedido);
        grupo.MapPost("/{id:guid}/cancelar", CancelarPedido);
    }

    private static async Task<IResult> ObterPedidos(ISender mediator)
    {
        var pedidos = await mediator.Send(new ObterPedidosConsulta());
        return Results.Ok(pedidos);
    }

    private static async Task<IResult> ObterPedidoPorId(Guid id, ISender mediator)
    {
        var pedido = await mediator.Send(new ObterPedidoConsulta(id));
        return pedido is null
            ? Results.NotFound(new { Mensagem = $"Pedido {id} não encontrado" })
            : Results.Ok(pedido);
    }

    private static async Task<IResult> CriarPedido(CriarPedidoComando comando, ISender mediator)
    {
        var idPedido = await mediator.Send(comando);
        return Results.Created($"/api/pedidos/{idPedido}", new { IdPedido = idPedido });
    }

    private static async Task<IResult> CancelarPedido(Guid id, ISender mediator)
    {
        var sucesso = await mediator.Send(new CancelarPedidoComando(id));
        return sucesso
            ? Results.Ok(new { Mensagem = "Pedido cancelado com sucesso" })
            : Results.BadRequest(new { Mensagem = "Não foi possível cancelar o pedido" });
    }
}
