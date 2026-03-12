using Microsoft.EntityFrameworkCore;

namespace VendeTudo.Catalogo.API;

/// <summary>
/// Serviço hospedado para semeadura de dados do catálogo.
/// </summary>
public class SemeadorDadosCatalogo : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SemeadorDadosCatalogo> _logger;

    public SemeadorDadosCatalogo(IServiceProvider serviceProvider, ILogger<SemeadorDadosCatalogo> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CatalogoDbContext>();

        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (await context.Itens.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Catálogo já possui dados, pulando semeadura");
            return;
        }

        await SemearTiposAsync(context, cancellationToken);
        await SemearMarcasAsync(context, cancellationToken);
        await SemearItensAsync(context, cancellationToken);

        _logger.LogInformation("Semeadura do catálogo concluída");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task SemearTiposAsync(CatalogoDbContext context, CancellationToken ct)
    {
        var tipos = new[]
        {
            new TipoProduto { Id = 1, Tipo = "Camiseta" },
            new TipoProduto { Id = 2, Tipo = "Caneca" },
            new TipoProduto { Id = 3, Tipo = "Boné" },
            new TipoProduto { Id = 4, Tipo = "Adesivo" },
            new TipoProduto { Id = 5, Tipo = "Bolsa" }
        };

        await context.Tipos.AddRangeAsync(tipos, ct);
        await context.SaveChangesAsync(ct);
    }

    private async Task SemearMarcasAsync(CatalogoDbContext context, CancellationToken ct)
    {
        var marcas = new[]
        {
            new MarcaProduto { Id = 1, Marca = "VendeTudo Original" },
            new MarcaProduto { Id = 2, Marca = "TechBrasil" },
            new MarcaProduto { Id = 3, Marca = "CodeStyle" },
            new MarcaProduto { Id = 4, Marca = "DevWear" },
            new MarcaProduto { Id = 5, Marca = "OpenSource Gear" }
        };

        await context.Marcas.AddRangeAsync(marcas, ct);
        await context.SaveChangesAsync(ct);
    }

    private async Task SemearItensAsync(CatalogoDbContext context, CancellationToken ct)
    {
        var itens = new[]
        {
            new ItemCatalogo
            {
                Nome = "Camiseta .NET Developer",
                Descricao = "Camiseta 100% algodão com estampa .NET",
                Preco = 79.90m,
                UrlImagem = "/imagens/camiseta-dotnet.png",
                QuantidadeEstoque = 100,
                IdTipoProduto = 1,
                IdMarcaProduto = 1
            },
            new ItemCatalogo
            {
                Nome = "Caneca C# Code",
                Descricao = "Caneca de porcelana com código C#",
                Preco = 39.90m,
                UrlImagem = "/imagens/caneca-csharp.png",
                QuantidadeEstoque = 50,
                IdTipoProduto = 2,
                IdMarcaProduto = 2
            },
            new ItemCatalogo
            {
                Nome = "Boné DevOps",
                Descricao = "Boné ajustável com logo DevOps",
                Preco = 59.90m,
                UrlImagem = "/imagens/bone-devops.png",
                QuantidadeEstoque = 30,
                IdTipoProduto = 3,
                IdMarcaProduto = 3
            },
            new ItemCatalogo
            {
                Nome = "Adesivo Pack Programador",
                Descricao = "Pack com 10 adesivos de programação",
                Preco = 19.90m,
                UrlImagem = "/imagens/adesivos-pack.png",
                QuantidadeEstoque = 200,
                IdTipoProduto = 4,
                IdMarcaProduto = 4
            },
            new ItemCatalogo
            {
                Nome = "Bolsa Notebook Developer",
                Descricao = "Bolsa para notebook até 15 polegadas",
                Preco = 149.90m,
                UrlImagem = "/imagens/bolsa-notebook.png",
                QuantidadeEstoque = 25,
                IdTipoProduto = 5,
                IdMarcaProduto = 5
            },
            new ItemCatalogo
            {
                Nome = "Camiseta Kubernetes",
                Descricao = "Camiseta com logo Kubernetes",
                Preco = 89.90m,
                UrlImagem = "/imagens/camiseta-k8s.png",
                QuantidadeEstoque = 80,
                IdTipoProduto = 1,
                IdMarcaProduto = 3
            },
            new ItemCatalogo
            {
                Nome = "Caneca Docker",
                Descricao = "Caneca térmica com logo Docker",
                Preco = 49.90m,
                UrlImagem = "/imagens/caneca-docker.png",
                QuantidadeEstoque = 40,
                IdTipoProduto = 2,
                IdMarcaProduto = 2
            },
            new ItemCatalogo
            {
                Nome = "Boné GitHub",
                Descricao = "Boné snapback com logo GitHub",
                Preco = 69.90m,
                UrlImagem = "/imagens/bone-github.png",
                QuantidadeEstoque = 35,
                IdTipoProduto = 3,
                IdMarcaProduto = 5
            },
            new ItemCatalogo
            {
                Nome = "Adesivo .NET MAUI",
                Descricao = "Adesivo holográfico .NET MAUI",
                Preco = 9.90m,
                UrlImagem = "/imagens/adesivo-maui.png",
                QuantidadeEstoque = 150,
                IdTipoProduto = 4,
                IdMarcaProduto = 1
            },
            new ItemCatalogo
            {
                Nome = "Bolsa Crossbody Dev",
                Descricao = "Bolsa crossbody para desenvolvedores",
                Preco = 119.90m,
                UrlImagem = "/imagens/bolsa-crossbody.png",
                QuantidadeEstoque = 20,
                IdTipoProduto = 5,
                IdMarcaProduto = 4
            }
        };

        await context.Itens.AddRangeAsync(itens, ct);
        await context.SaveChangesAsync(ct);
    }
}
