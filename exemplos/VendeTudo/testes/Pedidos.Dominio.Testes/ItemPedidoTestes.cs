using FluentAssertions;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.Dominio.Testes;

public class ItemPedidoTestes
{
    [Fact]
    public void CriarItemPedido_DeveInicializarCorretamente()
    {
        // Act
        var item = new ItemPedido(1, "Produto Teste", 99.90m, "/img.png", 2);

        // Assert
        item.IdProduto.Should().Be(1);
        item.NomeProduto.Should().Be("Produto Teste");
        item.PrecoUnitario.Should().Be(99.90m);
        item.UrlImagem.Should().Be("/img.png");
        item.GetUnidades().Should().Be(2);
    }

    [Fact]
    public void CriarItemPedido_DeveLancarExcecaoParaUnidadesInvalidas()
    {
        // Act
        var act = () => new ItemPedido(1, "Produto", 100m, "/img.png", 0);

        // Assert
        act.Should().Throw<ExcecaoDominio>()
            .WithMessage("*unidades*");
    }

    [Fact]
    public void CriarItemPedido_DeveLancarExcecaoParaPrecoNegativo()
    {
        // Act
        var act = () => new ItemPedido(1, "Produto", -10m, "/img.png", 1);

        // Assert
        act.Should().Throw<ExcecaoDominio>()
            .WithMessage("*negativo*");
    }

    [Fact]
    public void AdicionarUnidades_DeveIncrementarCorretamente()
    {
        // Arrange
        var item = new ItemPedido(1, "Produto", 100m, "/img.png", 2);

        // Act
        item.AdicionarUnidades(3);

        // Assert
        item.GetUnidades().Should().Be(5);
    }

    [Fact]
    public void DefinirUnidades_DeveAlterarCorretamente()
    {
        // Arrange
        var item = new ItemPedido(1, "Produto", 100m, "/img.png", 2);

        // Act
        item.DefinirUnidades(10);

        // Assert
        item.GetUnidades().Should().Be(10);
    }
}
