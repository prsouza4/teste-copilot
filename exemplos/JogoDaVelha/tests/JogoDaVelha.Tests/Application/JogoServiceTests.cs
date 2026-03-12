namespace JogoDaVelha.Tests.Application;

using JogoDaVelha.Application.Services;
using JogoDaVelha.Domain.Enums;

public class JogoServiceTests
{
    [Fact]
    public void NovoJogo_DeveIniciarComJogadorX()
    {
        // Arrange & Act
        var jogo = new JogoService();

        // Assert
        Assert.Equal(Jogador.X, jogo.ObterJogadorAtual());
        Assert.True(jogo.JogoAtivo);
        Assert.Equal(EstadoJogo.EmAndamento, jogo.ObterEstado());
    }

    [Fact]
    public void IniciarJogo_DeveReiniciarTabuleiro()
    {
        // Arrange
        var jogo = new JogoService();
        jogo.FazerJogada(0, 0);
        jogo.FazerJogada(1, 1);

        // Act
        jogo.IniciarJogo();

        // Assert
        var tabuleiro = jogo.ObterTabuleiro();
        Assert.Equal(Jogador.Nenhum, tabuleiro[0, 0]);
        Assert.Equal(Jogador.Nenhum, tabuleiro[1, 1]);
        Assert.Equal(Jogador.X, jogo.ObterJogadorAtual());
    }

    [Fact]
    public void FazerJogada_Valida_DeveAlternarJogador()
    {
        // Arrange
        var jogo = new JogoService();

        // Act
        jogo.FazerJogada(0, 0);

        // Assert
        Assert.Equal(Jogador.O, jogo.ObterJogadorAtual());
    }

    [Fact]
    public void FazerJogada_Valida_DevePreencherCelula()
    {
        // Arrange
        var jogo = new JogoService();

        // Act
        jogo.FazerJogada(0, 0);

        // Assert
        var tabuleiro = jogo.ObterTabuleiro();
        Assert.Equal(Jogador.X, tabuleiro[0, 0]);
    }

    [Fact]
    public void FazerJogada_DuasJogadas_DeveAlternarCorretamente()
    {
        // Arrange
        var jogo = new JogoService();

        // Act
        jogo.FazerJogada(0, 0); // X
        jogo.FazerJogada(1, 1); // O

        // Assert
        Assert.Equal(Jogador.X, jogo.ObterJogadorAtual());
        var tabuleiro = jogo.ObterTabuleiro();
        Assert.Equal(Jogador.X, tabuleiro[0, 0]);
        Assert.Equal(Jogador.O, tabuleiro[1, 1]);
    }

    [Fact]
    public void FazerJogada_PosicaoOcupada_DeveRetornarFalse()
    {
        // Arrange
        var jogo = new JogoService();
        jogo.FazerJogada(0, 0);

        // Act
        var resultado = jogo.FazerJogada(0, 0);

        // Assert
        Assert.False(resultado);
        Assert.Equal(Jogador.O, jogo.ObterJogadorAtual());
    }

    [Fact]
    public void FazerJogada_AposVitoria_DeveRetornarFalse()
    {
        // Arrange
        var jogo = new JogoService();
        // Vitória do X na linha 0
        jogo.FazerJogada(0, 0); // X
        jogo.FazerJogada(1, 0); // O
        jogo.FazerJogada(0, 1); // X
        jogo.FazerJogada(1, 1); // O
        jogo.FazerJogada(0, 2); // X vence

        // Act
        var resultado = jogo.FazerJogada(2, 2);

        // Assert
        Assert.False(resultado);
        Assert.False(jogo.JogoAtivo);
    }

    [Fact]
    public void VerificarVitoria_LinhaCompleta_DeveDetectarVitoria()
    {
        // Arrange
        var jogo = new JogoService();

        // Act - X vence na linha 0
        jogo.FazerJogada(0, 0); // X
        jogo.FazerJogada(1, 0); // O
        jogo.FazerJogada(0, 1); // X
        jogo.FazerJogada(1, 1); // O
        jogo.FazerJogada(0, 2); // X vence

        // Assert
        Assert.Equal(EstadoJogo.VitoriaX, jogo.ObterEstado());
        Assert.False(jogo.JogoAtivo);
    }

    [Fact]
    public void VerificarVitoria_ColunaCompleta_DeveDetectarVitoria()
    {
        // Arrange
        var jogo = new JogoService();

        // Act - X vence na coluna 0
        jogo.FazerJogada(0, 0); // X
        jogo.FazerJogada(0, 1); // O
        jogo.FazerJogada(1, 0); // X
        jogo.FazerJogada(0, 2); // O
        jogo.FazerJogada(2, 0); // X vence

        // Assert
        Assert.Equal(EstadoJogo.VitoriaX, jogo.ObterEstado());
    }

    [Fact]
    public void VerificarVitoria_DiagonalPrincipal_DeveDetectarVitoria()
    {
        // Arrange
        var jogo = new JogoService();

        // Act - X vence na diagonal principal
        jogo.FazerJogada(0, 0); // X
        jogo.FazerJogada(0, 1); // O
        jogo.FazerJogada(1, 1); // X
        jogo.FazerJogada(0, 2); // O
        jogo.FazerJogada(2, 2); // X vence

        // Assert
        Assert.Equal(EstadoJogo.VitoriaX, jogo.ObterEstado());
    }

    [Fact]
    public void VerificarVitoria_DiagonalSecundaria_DeveDetectarVitoria()
    {
        // Arrange
        var jogo = new JogoService();

        // Act - X vence na diagonal secundária
        jogo.FazerJogada(0, 2); // X
        jogo.FazerJogada(0, 0); // O
        jogo.FazerJogada(1, 1); // X
        jogo.FazerJogada(0, 1); // O
        jogo.FazerJogada(2, 0); // X vence

        // Assert
        Assert.Equal(EstadoJogo.VitoriaX, jogo.ObterEstado());
    }

    [Fact]
    public void VerificarVitoria_JogadorO_DeveDetectarVitoria()
    {
        // Arrange
        var jogo = new JogoService();

        // Act - O vence na linha 1
        jogo.FazerJogada(0, 0); // X
        jogo.FazerJogada(1, 0); // O
        jogo.FazerJogada(0, 1); // X
        jogo.FazerJogada(1, 1); // O
        jogo.FazerJogada(2, 2); // X
        jogo.FazerJogada(1, 2); // O vence

        // Assert
        Assert.Equal(EstadoJogo.VitoriaO, jogo.ObterEstado());
        Assert.False(jogo.JogoAtivo);
    }

    [Fact]
    public void VerificarEmpate_TabuleiroCompleto_DeveDetectarEmpate()
    {
        // Arrange
        var jogo = new JogoService();

        // Act - Empate
        // X | O | X
        // X | O | O
        // O | X | X
        jogo.FazerJogada(0, 0); // X
        jogo.FazerJogada(0, 1); // O
        jogo.FazerJogada(0, 2); // X
        jogo.FazerJogada(1, 1); // O
        jogo.FazerJogada(1, 0); // X
        jogo.FazerJogada(1, 2); // O
        jogo.FazerJogada(2, 1); // X
        jogo.FazerJogada(2, 0); // O
        jogo.FazerJogada(2, 2); // X

        // Assert
        Assert.Equal(EstadoJogo.Empate, jogo.ObterEstado());
        Assert.False(jogo.JogoAtivo);
    }

    [Fact]
    public void JogoAtivo_AposIniciar_DeveSerTrue()
    {
        // Arrange & Act
        var jogo = new JogoService();

        // Assert
        Assert.True(jogo.JogoAtivo);
    }

    [Fact]
    public void JogoAtivo_AposVitoria_DeveSerFalse()
    {
        // Arrange
        var jogo = new JogoService();
        jogo.FazerJogada(0, 0); // X
        jogo.FazerJogada(1, 0); // O
        jogo.FazerJogada(0, 1); // X
        jogo.FazerJogada(1, 1); // O
        jogo.FazerJogada(0, 2); // X vence

        // Assert
        Assert.False(jogo.JogoAtivo);
    }

    [Fact]
    public void ObterTabuleiro_DeveRetornarCopia()
    {
        // Arrange
        var jogo = new JogoService();
        jogo.FazerJogada(0, 0);

        // Act
        var tabuleiro = jogo.ObterTabuleiro();

        // Assert
        Assert.NotNull(tabuleiro);
        Assert.Equal(3, tabuleiro.GetLength(0));
        Assert.Equal(3, tabuleiro.GetLength(1));
        Assert.Equal(Jogador.X, tabuleiro[0, 0]);
    }
}
