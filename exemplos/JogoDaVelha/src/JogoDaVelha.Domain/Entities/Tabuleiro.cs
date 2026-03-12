namespace JogoDaVelha.Domain.Entities;

using JogoDaVelha.Domain.Enums;

/// <summary>
/// Representa o tabuleiro 3x3 do jogo da velha.
/// </summary>
public class Tabuleiro
{
    private const int Tamanho = 3;
    private readonly Jogador[,] _celulas;

    /// <summary>
    /// Número de jogadas realizadas no tabuleiro.
    /// </summary>
    public int JogadasRealizadas { get; private set; }

    /// <summary>
    /// Cria um novo tabuleiro vazio.
    /// </summary>
    public Tabuleiro()
    {
        _celulas = new Jogador[Tamanho, Tamanho];
        JogadasRealizadas = 0;
    }

    /// <summary>
    /// Verifica se uma célula está vazia.
    /// </summary>
    /// <param name="linha">Linha (0-2).</param>
    /// <param name="coluna">Coluna (0-2).</param>
    /// <returns>True se a célula está vazia.</returns>
    public bool EstaVazio(int linha, int coluna)
    {
        if (!PosicaoValida(linha, coluna))
        {
            return false;
        }

        return _celulas[linha, coluna] == Jogador.Nenhum;
    }

    /// <summary>
    /// Obtém o jogador em uma célula específica.
    /// </summary>
    /// <param name="linha">Linha (0-2).</param>
    /// <param name="coluna">Coluna (0-2).</param>
    /// <returns>O jogador na célula ou Nenhum se vazia.</returns>
    public Jogador ObterCelula(int linha, int coluna)
    {
        if (!PosicaoValida(linha, coluna))
        {
            return Jogador.Nenhum;
        }

        return _celulas[linha, coluna];
    }

    /// <summary>
    /// Realiza uma jogada no tabuleiro.
    /// </summary>
    /// <param name="linha">Linha (0-2).</param>
    /// <param name="coluna">Coluna (0-2).</param>
    /// <param name="jogador">O jogador que está fazendo a jogada.</param>
    /// <returns>True se a jogada foi válida e realizada.</returns>
    public bool FazerJogada(int linha, int coluna, Jogador jogador)
    {
        if (jogador == Jogador.Nenhum)
        {
            return false;
        }

        if (!PosicaoValida(linha, coluna))
        {
            return false;
        }

        if (!EstaVazio(linha, coluna))
        {
            return false;
        }

        _celulas[linha, coluna] = jogador;
        JogadasRealizadas++;
        return true;
    }

    /// <summary>
    /// Verifica o resultado atual do jogo.
    /// </summary>
    /// <returns>O estado atual do jogo.</returns>
    public EstadoJogo VerificarResultado()
    {
        // Verifica linhas
        for (int linha = 0; linha < Tamanho; linha++)
        {
            if (VerificarLinha(linha))
            {
                return ObterVitoria(_celulas[linha, 0]);
            }
        }

        // Verifica colunas
        for (int coluna = 0; coluna < Tamanho; coluna++)
        {
            if (VerificarColuna(coluna))
            {
                return ObterVitoria(_celulas[0, coluna]);
            }
        }

        // Verifica diagonal principal
        if (VerificarDiagonalPrincipal())
        {
            return ObterVitoria(_celulas[0, 0]);
        }

        // Verifica diagonal secundária
        if (VerificarDiagonalSecundaria())
        {
            return ObterVitoria(_celulas[0, Tamanho - 1]);
        }

        // Verifica empate
        if (JogadasRealizadas == Tamanho * Tamanho)
        {
            return EstadoJogo.Empate;
        }

        return EstadoJogo.EmAndamento;
    }

    /// <summary>
    /// Obtém uma cópia do tabuleiro como array 2D.
    /// </summary>
    /// <returns>Array 3x3 com o estado do tabuleiro.</returns>
    public Jogador[,] ObterCopia()
    {
        var copia = new Jogador[Tamanho, Tamanho];
        for (int i = 0; i < Tamanho; i++)
        {
            for (int j = 0; j < Tamanho; j++)
            {
                copia[i, j] = _celulas[i, j];
            }
        }
        return copia;
    }

    private bool PosicaoValida(int linha, int coluna)
    {
        return linha >= 0 && linha < Tamanho && coluna >= 0 && coluna < Tamanho;
    }

    private bool VerificarLinha(int linha)
    {
        var primeiro = _celulas[linha, 0];
        if (primeiro == Jogador.Nenhum)
        {
            return false;
        }

        return _celulas[linha, 1] == primeiro && _celulas[linha, 2] == primeiro;
    }

    private bool VerificarColuna(int coluna)
    {
        var primeiro = _celulas[0, coluna];
        if (primeiro == Jogador.Nenhum)
        {
            return false;
        }

        return _celulas[1, coluna] == primeiro && _celulas[2, coluna] == primeiro;
    }

    private bool VerificarDiagonalPrincipal()
    {
        var primeiro = _celulas[0, 0];
        if (primeiro == Jogador.Nenhum)
        {
            return false;
        }

        return _celulas[1, 1] == primeiro && _celulas[2, 2] == primeiro;
    }

    private bool VerificarDiagonalSecundaria()
    {
        var primeiro = _celulas[0, 2];
        if (primeiro == Jogador.Nenhum)
        {
            return false;
        }

        return _celulas[1, 1] == primeiro && _celulas[2, 0] == primeiro;
    }

    private static EstadoJogo ObterVitoria(Jogador jogador)
    {
        return jogador switch
        {
            Jogador.X => EstadoJogo.VitoriaX,
            Jogador.O => EstadoJogo.VitoriaO,
            _ => EstadoJogo.EmAndamento
        };
    }
}
