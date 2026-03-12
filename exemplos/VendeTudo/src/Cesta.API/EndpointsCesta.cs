using VendeTudo.BarramentoEventos;
using VendeTudo.Compartilhado;

namespace VendeTudo.Cesta.API;

/// <summary>
/// Endpoints da API de Cesta.
/// </summary>
public static class EndpointsCesta
{
    public static void MapearEndpointsCesta(this WebApplication app)
    {
        var grupo = app.MapGroup("/api/cesta");

        grupo.MapGet("/{idCliente}", ObterCesta);
        grupo.MapPost("/", AtualizarCesta);
        grupo.MapDelete("/{idCliente}", RemoverCesta);
        grupo.MapPost("/{idCliente}/checkout", RealizarCheckout);
    }

    private static async Task<IResult> ObterCesta(string idCliente, ICestaRepositorio repositorio)
    {
        var cesta = await repositorio.ObterCestaAsync(idCliente);
        return cesta is null
            ? Results.Ok(new CestaCliente { IdCliente = idCliente })
            : Results.Ok(cesta);
    }

    private static async Task<IResult> AtualizarCesta(CestaCliente cesta, ICestaRepositorio repositorio)
    {
        var cestaAtualizada = await repositorio.AtualizarCestaAsync(cesta);
        return cestaAtualizada is null
            ? Results.Problem("Falha ao atualizar cesta")
            : Results.Ok(cestaAtualizada);
    }

    private static async Task<IResult> RemoverCesta(string idCliente, ICestaRepositorio repositorio)
    {
        await repositorio.RemoverCestaAsync(idCliente);
        return Results.NoContent();
    }

    private static async Task<IResult> RealizarCheckout(
        string idCliente,
        CheckoutRequest request,
        ICestaRepositorio repositorio,
        IBarramentoEventos barramento)
    {
        var cesta = await repositorio.ObterCestaAsync(idCliente);
        if (cesta is null || cesta.Itens.Count == 0)
        {
            return Results.BadRequest(new { Mensagem = "Cesta vazia" });
        }

        var itensCheckout = cesta.Itens.Select(i => new ItemCheckout
        {
            IdProduto = i.IdProduto,
            NomeProduto = i.NomeProduto,
            PrecoUnitario = i.PrecoUnitario,
            Quantidade = i.Quantidade,
            UrlImagem = i.UrlImagem ?? string.Empty
        }).ToList();

        var enderecoCheckout = new EnderecoCheckout
        {
            Rua = request.Endereco.Rua,
            Cidade = request.Endereco.Cidade,
            Estado = request.Endereco.Estado,
            Pais = request.Endereco.Pais,
            Cep = request.Endereco.Cep
        };

        var evento = new CheckoutAceitoEventoIntegracao(idCliente, itensCheckout, enderecoCheckout);
        barramento.Publicar(evento);

        await repositorio.RemoverCestaAsync(idCliente);

        return Results.Accepted(null, new { Mensagem = "Checkout aceito", IdEvento = evento.Id });
    }
}

/// <summary>
/// Request para checkout.
/// </summary>
public class CheckoutRequest
{
    public EnderecoRequest Endereco { get; set; } = new();
}

/// <summary>
/// Endereço no request de checkout.
/// </summary>
public class EnderecoRequest
{
    public string Rua { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
}
