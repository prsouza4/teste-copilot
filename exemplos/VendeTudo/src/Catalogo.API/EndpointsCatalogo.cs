using Microsoft.EntityFrameworkCore;
using VendeTudo.BarramentoEventos;
using VendeTudo.Compartilhado;

namespace VendeTudo.Catalogo.API;

/// <summary>
/// Endpoints da API de Catálogo.
/// </summary>
public static class EndpointsCatalogo
{
    public static void MapearEndpointsCatalogo(this WebApplication app)
    {
        var grupo = app.MapGroup("/api/catalogo");

        grupo.MapGet("/itens", ObterItens);
        grupo.MapGet("/itens/{id:int}", ObterItemPorId);
        grupo.MapGet("/itens/tipo/{idTipo:int}", ObterItensPorTipo);
        grupo.MapGet("/itens/marca/{idMarca:int}", ObterItensPorMarca);
        grupo.MapGet("/tipos", ObterTipos);
        grupo.MapGet("/marcas", ObterMarcas);
        grupo.MapPost("/itens", CriarItem);
        grupo.MapPut("/itens/{id:int}", AtualizarItem);
        grupo.MapDelete("/itens/{id:int}", RemoverItem);
    }

    private static async Task<IResult> ObterItens(
        CatalogoDbContext context,
        int pagina = 1,
        int tamanhoPagina = 10)
    {
        var totalItens = await context.Itens.CountAsync();
        var itens = await context.Itens
            .Include(i => i.TipoProduto)
            .Include(i => i.MarcaProduto)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return Results.Ok(new
        {
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            TotalItens = totalItens,
            TotalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina),
            Itens = itens
        });
    }

    private static async Task<IResult> ObterItemPorId(int id, CatalogoDbContext context)
    {
        var item = await context.Itens
            .Include(i => i.TipoProduto)
            .Include(i => i.MarcaProduto)
            .FirstOrDefaultAsync(i => i.Id == id);

        return item is null
            ? Results.NotFound(new { Mensagem = $"Item com ID {id} não encontrado" })
            : Results.Ok(item);
    }

    private static async Task<IResult> ObterItensPorTipo(int idTipo, CatalogoDbContext context)
    {
        var itens = await context.Itens
            .Include(i => i.TipoProduto)
            .Include(i => i.MarcaProduto)
            .Where(i => i.IdTipoProduto == idTipo)
            .ToListAsync();

        return Results.Ok(itens);
    }

    private static async Task<IResult> ObterItensPorMarca(int idMarca, CatalogoDbContext context)
    {
        var itens = await context.Itens
            .Include(i => i.TipoProduto)
            .Include(i => i.MarcaProduto)
            .Where(i => i.IdMarcaProduto == idMarca)
            .ToListAsync();

        return Results.Ok(itens);
    }

    private static async Task<IResult> ObterTipos(CatalogoDbContext context)
    {
        var tipos = await context.Tipos.ToListAsync();
        return Results.Ok(tipos);
    }

    private static async Task<IResult> ObterMarcas(CatalogoDbContext context)
    {
        var marcas = await context.Marcas.ToListAsync();
        return Results.Ok(marcas);
    }

    private static async Task<IResult> CriarItem(
        ItemCatalogo item,
        CatalogoDbContext context)
    {
        context.Itens.Add(item);
        await context.SaveChangesAsync();

        return Results.Created($"/api/catalogo/itens/{item.Id}", item);
    }

    private static async Task<IResult> AtualizarItem(
        int id,
        ItemCatalogo itemAtualizado,
        CatalogoDbContext context,
        IBarramentoEventos barramento)
    {
        var item = await context.Itens.FindAsync(id);
        if (item is null)
        {
            return Results.NotFound(new { Mensagem = $"Item com ID {id} não encontrado" });
        }

        var precoAntigo = item.Preco;

        item.Nome = itemAtualizado.Nome;
        item.Descricao = itemAtualizado.Descricao;
        item.Preco = itemAtualizado.Preco;
        item.UrlImagem = itemAtualizado.UrlImagem;
        item.NomeArquivoImagem = itemAtualizado.NomeArquivoImagem;
        item.QuantidadeEstoque = itemAtualizado.QuantidadeEstoque;
        item.UnidadeLimiteCompra = itemAtualizado.UnidadeLimiteCompra;
        item.Disponivel = itemAtualizado.Disponivel;
        item.IdTipoProduto = itemAtualizado.IdTipoProduto;
        item.IdMarcaProduto = itemAtualizado.IdMarcaProduto;

        await context.SaveChangesAsync();

        if (precoAntigo != item.Preco)
        {
            var evento = new PrecoItemAlteradoEventoIntegracao(item.Id, precoAntigo, item.Preco);
            barramento.Publicar(evento);
        }

        return Results.Ok(item);
    }

    private static async Task<IResult> RemoverItem(int id, CatalogoDbContext context)
    {
        var item = await context.Itens.FindAsync(id);
        if (item is null)
        {
            return Results.NotFound(new { Mensagem = $"Item com ID {id} não encontrado" });
        }

        context.Itens.Remove(item);
        await context.SaveChangesAsync();

        return Results.NoContent();
    }
}
