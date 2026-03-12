namespace JogoDaVelha.Application.Services;

using JogoDaVelha.Application.Interfaces;
using JogoDaVelha.Domain.Entities;
using JogoDaVelha.Domain.Enums;

/// <summary>
/// Serviço que gerencia o fluxo do jogo da velha.
/// </summary>
public class JogoService : IJogoService
{
    private Tabuleiro _tabuleiro;
    private Jogador _jogadorAtual;
    private EstadoJogo _estadoAtual;

    /// <summary>
    /// Cria uma nova instância do serviço de jogo.
    /// </summary>
    public JogoService()
    {
        _tabuleiro = new Tabuleiro();
        _jogadorAtual = Jogador.X;
        _estadoAtual = EstadoJogo.EmAndamento;
    }

    /// <inheritdoc />
    public bool JogoAtivo => _estadoAtual == EstadoJogo.EmAndamento;

    /// <inheritdoc />
    public void IniciarJogo()
    {
        _tabuleiro = new Tabuleiro();
        _jogadorAtual = Jogador.X;
        _estadoAtual = EstadoJogo.EmAndamento;
    }

    /// <inheritdoc />
    public bool FazerJogada(int linha, int coluna)
    {
        if (!JogoAtivo)
        {
            return false;
        }

        var jogadaRealizada = _tabuleiro.FazerJogada(linha, coluna, _jogadorAtual);

        if (!jogadaRealizada)
        {
            return false;
        }

        _estadoAtual = _tabuleiro.VerificarResultado();

        if (JogoAtivo)
        {
            AlternarJogador();
        }

        return true;
    }

    /// <inheritdoc />
    public EstadoJogo ObterEstado()
    {
        return _estadoAtual;
    }

    /// <inheritdoc />
    public Jogador ObterJogadorAtual()
    {
        return _jogadorAtual;
    }

    /// <inheritdoc />
    public Jogador[,] ObterTabuleiro()
    {
        return _tabuleiro.ObterCopia();
    }

    private void AlternarJogador()
    {
        _jogadorAtual = _jogadorAtual == Jogador.X ? Jogador.O : Jogador.X;
    }
}
