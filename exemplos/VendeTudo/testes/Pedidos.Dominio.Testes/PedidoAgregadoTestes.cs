using FluentAssertions;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.Dominio.Testes;

public class PedidoAgregadoTestes
{
    [Fact]
    public void CriarPedido_DeveInicializarComStatusSubmetido()
    {
        // Arrange
        var idComprador = "comprador-123";
        var endereco = EnderecoEntrega.Criar("Rua Teste", "Cidade", "Estado", "Brasil", "12345-000");

        // Act
        var pedido = PedidoAgregado.CriarPedido(idComprador, endereco);

        // Assert
        pedido.Status.Should().Be(StatusPedido.Submetido);
        pedido.IdComprador.Should().Be(idComprador);
        pedido.Endereco.Should().Be(endereco);
        pedido.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void CriarPedido_DeveLancarExcecaoParaIdCompradorVazio()
    {
        // Arrange
        var endereco = EnderecoEntrega.Criar("Rua Teste", "Cidade", "Estado", "Brasil", "12345-000");

        // Act
        var act = () => PedidoAgregado.CriarPedido(string.Empty, endereco);

        // Assert
        act.Should().Throw<ExcecaoDominio>()
            .WithMessage("*comprador*");
    }

    [Fact]
    public void AdicionarItemPedido_DeveCalcularTotalCorretamente()
    {
        // Arrange
        var pedido = CriarPedidoValido();

        // Act
        pedido.AdicionarItemPedido(1, "Produto 1", 100m, "/img.png", 2);
        pedido.AdicionarItemPedido(2, "Produto 2", 50m, "/img2.png", 3);

        // Assert
        pedido.Total.Should().Be(350m); // (100 * 2) + (50 * 3) = 200 + 150 = 350
        pedido.Itens.Should().HaveCount(2);
    }

    [Fact]
    public void AdicionarItemPedido_DeveAdicionarUnidadesParaProdutoExistente()
    {
        // Arrange
        var pedido = CriarPedidoValido();
        pedido.AdicionarItemPedido(1, "Produto 1", 100m, "/img.png", 2);

        // Act
        pedido.AdicionarItemPedido(1, "Produto 1", 100m, "/img.png", 3);

        // Assert
        pedido.Itens.Should().HaveCount(1);
        pedido.Itens.First().GetUnidades().Should().Be(5);
        pedido.Total.Should().Be(500m);
    }

    [Fact]
    public void Cancelar_DeveLancarExcecaoSePedidoJaFoiEnviado()
    {
        // Arrange
        var pedido = CriarPedidoComStatusEnviado();

        // Act
        var act = () => pedido.Cancelar();

        // Assert
        act.Should().Throw<ExcecaoDominio>()
            .WithMessage("*enviado*");
    }

    [Fact]
    public void Cancelar_DeveAlterarStatusParaCancelado()
    {
        // Arrange
        var pedido = CriarPedidoValido();

        // Act
        pedido.Cancelar();

        // Assert
        pedido.Status.Should().Be(StatusPedido.Cancelado);
    }

    [Fact]
    public void Cancelar_DeveLancarExcecaoSePedidoJaEstaCancelado()
    {
        // Arrange
        var pedido = CriarPedidoValido();
        pedido.Cancelar();

        // Act
        var act = () => pedido.Cancelar();

        // Assert
        act.Should().Throw<ExcecaoDominio>()
            .WithMessage("*cancelado*");
    }

    [Fact]
    public void DefinirStatusAguardandoValidacao_DeveAlterarStatusCorreto()
    {
        // Arrange
        var pedido = CriarPedidoValido();

        // Act
        pedido.DefinirStatusAguardandoValidacao();

        // Assert
        pedido.Status.Should().Be(StatusPedido.AguardandoValidacao);
    }

    [Fact]
    public void DefinirStatusEstoqueConfirmado_DeveAlterarStatusCorreto()
    {
        // Arrange
        var pedido = CriarPedidoValido();
        pedido.DefinirStatusAguardandoValidacao();

        // Act
        pedido.DefinirStatusEstoqueConfirmado();

        // Assert
        pedido.Status.Should().Be(StatusPedido.EstoqueConfirmado);
    }

    [Fact]
    public void DefinirStatusPagoPendente_DeveAlterarStatusCorreto()
    {
        // Arrange
        var pedido = CriarPedidoValido();
        pedido.DefinirStatusAguardandoValidacao();
        pedido.DefinirStatusEstoqueConfirmado();

        // Act
        pedido.DefinirStatusPagoPendente();

        // Assert
        pedido.Status.Should().Be(StatusPedido.Pago);
    }

    [Fact]
    public void DefinirStatusEnviado_DeveAlterarStatusCorreto()
    {
        // Arrange
        var pedido = CriarPedidoValido();
        pedido.DefinirStatusAguardandoValidacao();
        pedido.DefinirStatusEstoqueConfirmado();
        pedido.DefinirStatusPagoPendente();

        // Act
        pedido.DefinirStatusEnviado();

        // Assert
        pedido.Status.Should().Be(StatusPedido.Enviado);
    }

    private static PedidoAgregado CriarPedidoValido()
    {
        var endereco = EnderecoEntrega.Criar("Rua Teste", "Cidade", "Estado", "Brasil", "12345-000");
        return PedidoAgregado.CriarPedido("comprador-123", endereco);
    }

    private static PedidoAgregado CriarPedidoComStatusEnviado()
    {
        var pedido = CriarPedidoValido();
        pedido.AdicionarItemPedido(1, "Produto", 100m, "/img.png", 1);
        pedido.DefinirStatusAguardandoValidacao();
        pedido.DefinirStatusEstoqueConfirmado();
        pedido.DefinirStatusPagoPendente();
        pedido.DefinirStatusEnviado();
        return pedido;
    }
}
