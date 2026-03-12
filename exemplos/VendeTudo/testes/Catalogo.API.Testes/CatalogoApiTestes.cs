using FluentAssertions;
using System.Net;
using VendeTudo.Catalogo.API;

namespace VendeTudo.Catalogo.API.Testes;

/// <summary>
/// Testes de unidade para validar a estrutura das entidades do catálogo.
/// </summary>
public class CatalogoEntidadesTestes
{
    [Fact]
    public void ItemCatalogo_DeveInicializarCorretamente()
    {
        // Arrange & Act
        var item = new ItemCatalogo
        {
            Id = 1,
            Nome = "Produto Teste",
            Descricao = "Descrição teste",
            Preco = 99.90m,
            QuantidadeEstoque = 10,
            Disponivel = true,
            IdTipoProduto = 1,
            IdMarcaProduto = 1
        };

        // Assert
        item.Id.Should().Be(1);
        item.Nome.Should().Be("Produto Teste");
        item.Preco.Should().Be(99.90m);
        item.QuantidadeEstoque.Should().Be(10);
        item.Disponivel.Should().BeTrue();
    }

    [Fact]
    public void TipoProduto_DeveInicializarCorretamente()
    {
        // Arrange & Act
        var tipo = new TipoProduto
        {
            Id = 1,
            Tipo = "Camiseta"
        };

        // Assert
        tipo.Id.Should().Be(1);
        tipo.Tipo.Should().Be("Camiseta");
    }

    [Fact]
    public void MarcaProduto_DeveInicializarCorretamente()
    {
        // Arrange & Act
        var marca = new MarcaProduto
        {
            Id = 1,
            Marca = "VendeTudo Original"
        };

        // Assert
        marca.Id.Should().Be(1);
        marca.Marca.Should().Be("VendeTudo Original");
    }

    [Fact]
    public void ItemCatalogo_UnidadeLimiteCompra_DeveTerValorPadrao()
    {
        // Arrange & Act
        var item = new ItemCatalogo();

        // Assert
        item.UnidadeLimiteCompra.Should().Be(10);
    }

    [Fact]
    public void ItemCatalogo_Disponivel_DeveTerValorPadraoTrue()
    {
        // Arrange & Act
        var item = new ItemCatalogo();

        // Assert
        item.Disponivel.Should().BeTrue();
    }
}
