namespace JogoDaVelha.Domain.Enums;

/// <summary>
/// Representa o estado atual do jogo.
/// </summary>
public enum EstadoJogo
{
    /// <summary>
    /// Jogo em andamento.
    /// </summary>
    EmAndamento,

    /// <summary>
    /// Jogador X venceu.
    /// </summary>
    VitoriaX,

    /// <summary>
    /// Jogador O venceu.
    /// </summary>
    VitoriaO,

    /// <summary>
    /// Empate (velha).
    /// </summary>
    Empate
}
