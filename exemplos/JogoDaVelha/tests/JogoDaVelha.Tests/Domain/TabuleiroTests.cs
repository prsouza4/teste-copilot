namespace JogoDaVelha.Tests.Domain;

using JogoDaVelha.Domain.Entities;
using JogoDaVelha.Domain.Enums;

public class TabuleiroTests
{
    [Fact]
    public void NovoTabuleiro_DeveEstarVazio()
    {
        // Arrange & Act
        var tabuleiro = new Tabuleiro();

        // Assert
        Assert.Equal(0, tabuleiro.JogadasRealizadas);
        Assert.Equal(EstadoJogo.EmAndamento, tabuleiro.VerificarResultado());
    }

    [Fact]
    public void NovoTabuleiro_TodasCelulasDevemEstarVazias()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();

        // Act & Assert
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Assert.True(tabuleiro.EstaVazio(i, j));
                Assert.Equal(Jogador.Nenhum, tabuleiro.ObterCelula(i, j));
            }
        }
    }

    [Fact]
    public void FazerJogada_PosicaoValida_DeveRetornarTrue()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();

        // Act
        var resultado = tabuleiro.FazerJogada(0, 0, Jogador.X);

        // Assert
        Assert.True(resultado);
        Assert.Equal(1, tabuleiro.JogadasRealizadas);
        Assert.Equal(Jogador.X, tabuleiro.ObterCelula(0, 0));
    }

    [Fact]
    public void FazerJogada_PosicaoOcupada_DeveRetornarFalse()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();
        tabuleiro.FazerJogada(0, 0, Jogador.X);

        // Act
        var resultado = tabuleiro.FazerJogada(0, 0, Jogador.O);

        // Assert
        Assert.False(resultado);
        Assert.Equal(1, tabuleiro.JogadasRealizadas);
        Assert.Equal(Jogador.X, tabuleiro.ObterCelula(0, 0));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(3, 0)]
    [InlineData(0, 3)]
    [InlineData(-1, -1)]
    [InlineData(3, 3)]
    public void FazerJogada_PosicaoForaDoLimite_DeveRetornarFalse(int linha, int coluna)
    {
        // Arrange
        var tabuleiro = new Tabuleiro();

        // Act
        var resultado = tabuleiro.FazerJogada(linha, coluna, Jogador.X);

        // Assert
        Assert.False(resultado);
        Assert.Equal(0, tabuleiro.JogadasRealizadas);
    }

    [Fact]
    public void FazerJogada_JogadorNenhum_DeveRetornarFalse()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();

        // Act
        var resultado = tabuleiro.FazerJogada(0, 0, Jogador.Nenhum);

        // Assert
        Assert.False(resultado);
        Assert.Equal(0, tabuleiro.JogadasRealizadas);
    }

    [Theory]
    [InlineData(0)] // Linha 0
    [InlineData(1)] // Linha 1
    [InlineData(2)] // Linha 2
    public void VerificarResultado_VitoriaHorizontalX_DeveRetornarVitoriaX(int linha)
    {
        // Arrange
        var tabuleiro = new Tabuleiro();
        tabuleiro.FazerJogada(linha, 0, Jogador.X);
        tabuleiro.FazerJogada(linha, 1, Jogador.X);
        tabuleiro.FazerJogada(linha, 2, Jogador.X);

        // Act
        var resultado = tabuleiro.VerificarResultado();

        // Assert
        Assert.Equal(EstadoJogo.VitoriaX, resultado);
    }

    [Theory]
    [InlineData(0)] // Coluna 0
    [InlineData(1)] // Coluna 1
    [InlineData(2)] // Coluna 2
    public void VerificarResultado_VitoriaVerticalX_DeveRetornarVitoriaX(int coluna)
    {
        // Arrange
        var tabuleiro = new Tabuleiro();
        tabuleiro.FazerJogada(0, coluna, Jogador.X);
        tabuleiro.FazerJogada(1, coluna, Jogador.X);
        tabuleiro.FazerJogada(2, coluna, Jogador.X);

        // Act
        var resultado = tabuleiro.VerificarResultado();

        // Assert
        Assert.Equal(EstadoJogo.VitoriaX, resultado);
    }

    [Fact]
    public void VerificarResultado_VitoriaDiagonalPrincipalX_DeveRetornarVitoriaX()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();
        tabuleiro.FazerJogada(0, 0, Jogador.X);
        tabuleiro.FazerJogada(1, 1, Jogador.X);
        tabuleiro.FazerJogada(2, 2, Jogador.X);

        // Act
        var resultado = tabuleiro.VerificarResultado();

        // Assert
        Assert.Equal(EstadoJogo.VitoriaX, resultado);
    }

    [Fact]
    public void VerificarResultado_VitoriaDiagonalSecundariaX_DeveRetornarVitoriaX()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();
        tabuleiro.FazerJogada(0, 2, Jogador.X);
        tabuleiro.FazerJogada(1, 1, Jogador.X);
        tabuleiro.FazerJogada(2, 0, Jogador.X);

        // Act
        var resultado = tabuleiro.VerificarResultado();

        // Assert
        Assert.Equal(EstadoJogo.VitoriaX, resultado);
    }

    [Fact]
    public void VerificarResultado_VitoriaHorizontalO_DeveRetornarVitoriaO()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();
        tabuleiro.FazerJogada(1, 0, Jogador.O);
        tabuleiro.FazerJogada(1, 1, Jogador.O);
        tabuleiro.FazerJogada(1, 2, Jogador.O);

        // Act
        var resultado = tabuleiro.VerificarResultado();

        // Assert
        Assert.Equal(EstadoJogo.VitoriaO, resultado);
    }

    [Fact]
    public void VerificarResultado_Empate_DeveRetornarEmpate()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();
        // X | O | X
        // X | X | O
        // O | X | O
        tabuleiro.FazerJogada(0, 0, Jogador.X);
        tabuleiro.FazerJogada(0, 1, Jogador.O);
        tabuleiro.FazerJogada(0, 2, Jogador.X);
        tabuleiro.FazerJogada(1, 0, Jogador.X);
        tabuleiro.FazerJogada(1, 1, Jogador.X);
        tabuleiro.FazerJogada(1, 2, Jogador.O);
        tabuleiro.FazerJogada(2, 0, Jogador.O);
        tabuleiro.FazerJogada(2, 1, Jogador.X);
        tabuleiro.FazerJogada(2, 2, Jogador.O);

        // Act
        var resultado = tabuleiro.VerificarResultado();

        // Assert
        Assert.Equal(EstadoJogo.Empate, resultado);
        Assert.Equal(9, tabuleiro.JogadasRealizadas);
    }

    [Fact]
    public void VerificarResultado_JogoEmAndamento_DeveRetornarEmAndamento()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();
        tabuleiro.FazerJogada(0, 0, Jogador.X);
        tabuleiro.FazerJogada(1, 1, Jogador.O);

        // Act
        var resultado = tabuleiro.VerificarResultado();

        // Assert
        Assert.Equal(EstadoJogo.EmAndamento, resultado);
    }

    [Fact]
    public void EstaVazio_PosicaoForaDoLimite_DeveRetornarFalse()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();

        // Act & Assert
        Assert.False(tabuleiro.EstaVazio(-1, 0));
        Assert.False(tabuleiro.EstaVazio(3, 0));
    }

    [Fact]
    public void ObterCelula_PosicaoForaDoLimite_DeveRetornarNenhum()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();

        // Act & Assert
        Assert.Equal(Jogador.Nenhum, tabuleiro.ObterCelula(-1, 0));
        Assert.Equal(Jogador.Nenhum, tabuleiro.ObterCelula(3, 0));
    }

    [Fact]
    public void ObterCopia_DeveRetornarCopiaDosValores()
    {
        // Arrange
        var tabuleiro = new Tabuleiro();
        tabuleiro.FazerJogada(0, 0, Jogador.X);
        tabuleiro.FazerJogada(1, 1, Jogador.O);

        // Act
        var copia = tabuleiro.ObterCopia();

        // Assert
        Assert.Equal(Jogador.X, copia[0, 0]);
        Assert.Equal(Jogador.O, copia[1, 1]);
        Assert.Equal(Jogador.Nenhum, copia[0, 1]);
    }
}
