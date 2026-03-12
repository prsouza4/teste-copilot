namespace JogoDaVelha.Application.Interfaces;

using JogoDaVelha.Domain.Enums;

/// <summary>
/// Interface para o serviço de gerenciamento do jogo da velha.
/// </summary>
public interface IJogoService
{
    /// <summary>
    /// Indica se o jogo está ativo (em andamento).
    /// </summary>
    bool JogoAtivo { get; }

    /// <summary>
    /// Inicia um novo jogo.
    /// </summary>
    void IniciarJogo();

    /// <summary>
    /// Realiza uma jogada na posição especificada.
    /// </summary>
    /// <param name="linha">Linha (0-2).</param>
    /// <param name="coluna">Coluna (0-2).</param>
    /// <returns>True se a jogada foi válida e realizada.</returns>
    bool FazerJogada(int linha, int coluna);

    /// <summary>
    /// Obtém o estado atual do jogo.
    /// </summary>
    /// <returns>O estado atual do jogo.</returns>
    EstadoJogo ObterEstado();

    /// <summary>
    /// Obtém o jogador atual (quem deve fazer a próxima jogada).
    /// </summary>
    /// <returns>O jogador atual.</returns>
    Jogador ObterJogadorAtual();

    /// <summary>
    /// Obtém uma cópia do tabuleiro atual.
    /// </summary>
    /// <returns>Array 3x3 representando o tabuleiro.</returns>
    Jogador[,] ObterTabuleiro();
}
