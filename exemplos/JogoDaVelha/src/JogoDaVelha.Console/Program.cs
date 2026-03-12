using JogoDaVelha.Application.Interfaces;
using JogoDaVelha.Application.Services;
using JogoDaVelha.Domain.Enums;

var jogoService = new JogoService();
ExecutarJogo(jogoService);

static void ExecutarJogo(IJogoService jogo)
{
    Console.Clear();
    Console.WriteLine("=================================");
    Console.WriteLine("       JOGO DA VELHA");
    Console.WriteLine("=================================");
    Console.WriteLine();

    jogo.IniciarJogo();

    while (jogo.JogoAtivo)
    {
        ExibirTabuleiro(jogo.ObterTabuleiro());
        Console.WriteLine();
        Console.WriteLine($"Vez do jogador: {jogo.ObterJogadorAtual()}");
        Console.WriteLine();
        Console.WriteLine("Escolha uma posição (1-9):");
        Console.WriteLine();
        ExibirPosicoes();
        Console.WriteLine();

        var posicao = LerPosicao();

        if (posicao < 1 || posicao > 9)
        {
            Console.WriteLine("Posição inválida! Use números de 1 a 9.");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey(true);
            Console.Clear();
            continue;
        }

        var (linha, coluna) = ConverterPosicao(posicao);

        if (!jogo.FazerJogada(linha, coluna))
        {
            Console.WriteLine("Jogada inválida! Posição já ocupada.");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey(true);
        }

        Console.Clear();
    }

    ExibirResultadoFinal(jogo);
}

static void ExibirTabuleiro(Jogador[,] tabuleiro)
{
    Console.WriteLine("  Tabuleiro atual:");
    Console.WriteLine();
    Console.WriteLine("     |     |     ");
    Console.WriteLine($"  {ObterSimbolo(tabuleiro[0, 0])}  |  {ObterSimbolo(tabuleiro[0, 1])}  |  {ObterSimbolo(tabuleiro[0, 2])}  ");
    Console.WriteLine("_____|_____|_____");
    Console.WriteLine("     |     |     ");
    Console.WriteLine($"  {ObterSimbolo(tabuleiro[1, 0])}  |  {ObterSimbolo(tabuleiro[1, 1])}  |  {ObterSimbolo(tabuleiro[1, 2])}  ");
    Console.WriteLine("_____|_____|_____");
    Console.WriteLine("     |     |     ");
    Console.WriteLine($"  {ObterSimbolo(tabuleiro[2, 0])}  |  {ObterSimbolo(tabuleiro[2, 1])}  |  {ObterSimbolo(tabuleiro[2, 2])}  ");
    Console.WriteLine("     |     |     ");
}

static void ExibirPosicoes()
{
    Console.WriteLine("  1  |  2  |  3  ");
    Console.WriteLine("-----|-----|-----");
    Console.WriteLine("  4  |  5  |  6  ");
    Console.WriteLine("-----|-----|-----");
    Console.WriteLine("  7  |  8  |  9  ");
}

static string ObterSimbolo(Jogador jogador)
{
    return jogador switch
    {
        Jogador.X => "X",
        Jogador.O => "O",
        _ => " "
    };
}

static int LerPosicao()
{
    Console.Write("Posição: ");
    var input = Console.ReadLine();

    if (int.TryParse(input, out var posicao))
    {
        return posicao;
    }

    return -1;
}

static (int linha, int coluna) ConverterPosicao(int posicao)
{
    return posicao switch
    {
        1 => (0, 0),
        2 => (0, 1),
        3 => (0, 2),
        4 => (1, 0),
        5 => (1, 1),
        6 => (1, 2),
        7 => (2, 0),
        8 => (2, 1),
        9 => (2, 2),
        _ => (-1, -1)
    };
}

static void ExibirResultadoFinal(IJogoService jogo)
{
    Console.WriteLine("=================================");
    Console.WriteLine("        FIM DE JOGO!");
    Console.WriteLine("=================================");
    Console.WriteLine();

    ExibirTabuleiro(jogo.ObterTabuleiro());
    Console.WriteLine();

    var estado = jogo.ObterEstado();
    var mensagem = estado switch
    {
        EstadoJogo.VitoriaX => "Jogador X venceu!",
        EstadoJogo.VitoriaO => "Jogador O venceu!",
        EstadoJogo.Empate => "Empate! Deu velha!",
        _ => "Jogo encerrado."
    };

    Console.WriteLine($"  Resultado: {mensagem}");
    Console.WriteLine();
    Console.WriteLine("Obrigado por jogar!");
}
