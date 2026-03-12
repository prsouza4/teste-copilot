using FluentAssertions;
using VendeTudo.Pedidos.API.Comandos;
using VendeTudo.Pedidos.API.Consultas;

namespace VendeTudo.Pedidos.API.Testes;

/// <summary>
/// Testes de unidade para validar a estrutura dos comandos e consultas da API.
/// </summary>
public class PedidosComandosTestes
{
    [Fact]
    public void CriarPedidoComando_DeveInicializarCorretamente()
    {
        // Arrange & Act
        var comando = new CriarPedidoComando(
            IdComprador: "comprador-teste",
            Rua: "Rua Teste",
            Cidade: "Cidade Teste",
            Estado: "SP",
            Pais: "Brasil",
            Cep: "12345-000",
            Itens: new List<ItemPedidoDto>
            {
                new ItemPedidoDto(
                    IdProduto: 1,
                    NomeProduto: "Produto Teste",
                    PrecoUnitario: 99.90m,
                    UrlImagem: "/img.png",
                    Quantidade: 2
                )
            }
        );

        // Assert
        comando.IdComprador.Should().Be("comprador-teste");
        comando.Rua.Should().Be("Rua Teste");
        comando.Cidade.Should().Be("Cidade Teste");
        comando.Estado.Should().Be("SP");
        comando.Pais.Should().Be("Brasil");
        comando.Cep.Should().Be("12345-000");
        comando.Itens.Should().HaveCount(1);
    }

    [Fact]
    public void CancelarPedidoComando_DeveInicializarCorretamente()
    {
        // Arrange
        var idPedido = Guid.NewGuid();

        // Act
        var comando = new CancelarPedidoComando(idPedido);

        // Assert
        comando.IdPedido.Should().Be(idPedido);
    }

    [Fact]
    public void ItemPedidoDto_DeveInicializarCorretamente()
    {
        // Arrange & Act
        var item = new ItemPedidoDto(
            IdProduto: 1,
            NomeProduto: "Produto Teste",
            PrecoUnitario: 99.90m,
            UrlImagem: "/img.png",
            Quantidade: 2
        );

        // Assert
        item.IdProduto.Should().Be(1);
        item.NomeProduto.Should().Be("Produto Teste");
        item.PrecoUnitario.Should().Be(99.90m);
        item.UrlImagem.Should().Be("/img.png");
        item.Quantidade.Should().Be(2);
    }

    [Fact]
    public void ObterPedidosConsulta_DeveSerInstanciavel()
    {
        // Arrange & Act
        var consulta = new ObterPedidosConsulta();

        // Assert
        consulta.Should().NotBeNull();
    }

    [Fact]
    public void ObterPedidoConsulta_DeveInicializarCorretamente()
    {
        // Arrange
        var idPedido = Guid.NewGuid();

        // Act
        var consulta = new ObterPedidoConsulta(idPedido);

        // Assert
        consulta.IdPedido.Should().Be(idPedido);
    }

    [Fact]
    public void PedidoDto_DeveInicializarCorretamente()
    {
        // Arrange & Act
        var dto = new PedidoDto(
            Id: Guid.NewGuid(),
            IdComprador: "comprador-teste",
            Status: "Submetido",
            DataCriacao: DateTime.UtcNow,
            Total: 199.80m,
            Endereco: new EnderecoDto("Rua Teste", "Cidade", "Estado", "Brasil", "12345-000"),
            Itens: new List<ItemPedidoResponseDto>
            {
                new ItemPedidoResponseDto(1, "Produto", 99.90m, 2, "/img.png")
            }
        );

        // Assert
        dto.IdComprador.Should().Be("comprador-teste");
        dto.Status.Should().Be("Submetido");
        dto.Total.Should().Be(199.80m);
        dto.Endereco.Rua.Should().Be("Rua Teste");
        dto.Itens.Should().HaveCount(1);
    }
}
