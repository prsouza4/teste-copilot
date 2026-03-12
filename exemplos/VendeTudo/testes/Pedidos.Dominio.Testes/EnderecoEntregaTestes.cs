using FluentAssertions;
using VendeTudo.Pedidos.Dominio;

namespace VendeTudo.Pedidos.Dominio.Testes;

public class EnderecoEntregaTestes
{
    [Fact]
    public void Criar_DeveRetornarEnderecoValido()
    {
        // Act
        var endereco = EnderecoEntrega.Criar("Rua Teste", "Cidade", "Estado", "Brasil", "12345-000");

        // Assert
        endereco.Rua.Should().Be("Rua Teste");
        endereco.Cidade.Should().Be("Cidade");
        endereco.Estado.Should().Be("Estado");
        endereco.Pais.Should().Be("Brasil");
        endereco.Cep.Should().Be("12345-000");
    }

    [Fact]
    public void Criar_DeveLancarExcecaoParaRuaVazia()
    {
        // Act
        var act = () => EnderecoEntrega.Criar("", "Cidade", "Estado", "Brasil", "12345-000");

        // Assert
        act.Should().Throw<ExcecaoDominio>()
            .WithMessage("*Rua*");
    }

    [Fact]
    public void Criar_DeveLancarExcecaoParaCidadeVazia()
    {
        // Act
        var act = () => EnderecoEntrega.Criar("Rua", "", "Estado", "Brasil", "12345-000");

        // Assert
        act.Should().Throw<ExcecaoDominio>()
            .WithMessage("*Cidade*");
    }

    [Fact]
    public void Criar_DeveLancarExcecaoParaCepVazio()
    {
        // Act
        var act = () => EnderecoEntrega.Criar("Rua", "Cidade", "Estado", "Brasil", "");

        // Assert
        act.Should().Throw<ExcecaoDominio>()
            .WithMessage("*CEP*");
    }
}
