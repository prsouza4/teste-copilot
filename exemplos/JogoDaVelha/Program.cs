using System;

namespace JogoDaVelha
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool jogarNovamente = true;

            while (jogarNovamente)
            {
                Jogo jogo = new Jogo();
                jogo.Iniciar();
                jogarNovamente = PerguntarJogarNovamente();
                Console.Clear();
            }

            Console.WriteLine("Obrigado por jogar! Até a próxima!");
        }

        static bool PerguntarJogarNovamente()
        {
            while (true)
            {
                Console.Write("\nDeseja jogar novamente? (S/N): ");
                string? resposta = Console.ReadLine()?.Trim().ToUpper();
                if (resposta == "S") return true;
                if (resposta == "N") return false;
                Console.WriteLine("Opção inválida. Digite S para sim ou N para não.");
            }
        }
    }

    enum ModoJogo { JogadorContraJogador, JogadorContraMaquina }
    enum NivelDificuldade { Facil, Medio, Dificil }

    class Jogo
    {
        private const int DelayJogadaMaquinaMs = 600;
        private const int DelayMensagemErroMs = 1200;
        private const double ProbabilidadeEstrategicaMedio = 0.6;

        private char[] _tabuleiro = new char[9];
        private char _jogadorAtual = 'X';
        private ModoJogo _modo;
        private NivelDificuldade _nivel;
        private Random _random = new Random();

        public void Iniciar()
        {
            InicializarTabuleiro();
            _modo = EscolherModoDeJogo();

            if (_modo == ModoJogo.JogadorContraMaquina)
                _nivel = EscolherNivelDificuldade();

            Console.Clear();
            bool jogoAtivo = true;

            while (jogoAtivo)
            {
                ExibirTabuleiro();

                bool jogadaValida;
                if (_modo == ModoJogo.JogadorContraMaquina && _jogadorAtual == 'O')
                {
                    Console.WriteLine("\n🤖 Vez da máquina (O)...");
                    System.Threading.Thread.Sleep(DelayJogadaMaquinaMs);
                    JogadaMaquina();
                    jogadaValida = true;
                }
                else
                {
                    string nomeJogador = _modo == ModoJogo.JogadorContraJogador
                        ? $"Jogador {(_jogadorAtual == 'X' ? 1 : 2)} ({_jogadorAtual})"
                        : $"Jogador ({_jogadorAtual})";
                    jogadaValida = PedirJogada(nomeJogador);
                }

                if (!jogadaValida) continue;

                if (VerificarVitoria(_jogadorAtual))
                {
                    Console.Clear();
                    ExibirTabuleiro();
                    if (_modo == ModoJogo.JogadorContraMaquina && _jogadorAtual == 'O')
                        Console.WriteLine("\n🤖 A máquina venceu! Melhor sorte na próxima vez.");
                    else if (_modo == ModoJogo.JogadorContraJogador)
                        Console.WriteLine($"\n🎉 Jogador {(_jogadorAtual == 'X' ? 1 : 2)} ({_jogadorAtual}) venceu! Parabéns!");
                    else
                        Console.WriteLine($"\n🎉 Você venceu! Parabéns!");
                    jogoAtivo = false;
                }
                else if (VerificarEmpate())
                {
                    Console.Clear();
                    ExibirTabuleiro();
                    Console.WriteLine("\n🤝 Empate! Nenhum jogador venceu.");
                    jogoAtivo = false;
                }
                else
                {
                    AlternarJogador();
                }
            }
        }

        private void InicializarTabuleiro()
        {
            for (int i = 0; i < 9; i++)
                _tabuleiro[i] = (char)('1' + i);
        }

        private ModoJogo EscolherModoDeJogo()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║       JOGO DA VELHA 🎮        ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("Escolha o modo de jogo:");
            Console.WriteLine("  1 - Jogador vs Jogador");
            Console.WriteLine("  2 - Jogador vs Máquina");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Opção: ");
                string? input = Console.ReadLine()?.Trim();
                if (input == "1") return ModoJogo.JogadorContraJogador;
                if (input == "2") return ModoJogo.JogadorContraMaquina;
                Console.WriteLine("Opção inválida. Digite 1 ou 2.");
            }
        }

        private NivelDificuldade EscolherNivelDificuldade()
        {
            Console.Clear();
            Console.WriteLine("Escolha o nível de dificuldade da máquina:");
            Console.WriteLine("  1 - Fácil   (jogadas aleatórias)");
            Console.WriteLine("  2 - Médio   (às vezes joga estrategicamente)");
            Console.WriteLine("  3 - Difícil (joga de forma ótima — impossível vencer!)");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Opção: ");
                string? input = Console.ReadLine()?.Trim();
                if (input == "1") return NivelDificuldade.Facil;
                if (input == "2") return NivelDificuldade.Medio;
                if (input == "3") return NivelDificuldade.Dificil;
                Console.WriteLine("Opção inválida. Digite 1, 2 ou 3.");
            }
        }

        private void ExibirTabuleiro()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║       JOGO DA VELHA 🎮        ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.WriteLine();

            if (_modo == ModoJogo.JogadorContraMaquina)
                Console.WriteLine($"  Dificuldade: {NomeDificuldade(_nivel)}");

            Console.WriteLine();
            Console.WriteLine($"  {CelulaFormatada(0)} │ {CelulaFormatada(1)} │ {CelulaFormatada(2)}");
            Console.WriteLine("  ──┼───┼──");
            Console.WriteLine($"  {CelulaFormatada(3)} │ {CelulaFormatada(4)} │ {CelulaFormatada(5)}");
            Console.WriteLine("  ──┼───┼──");
            Console.WriteLine($"  {CelulaFormatada(6)} │ {CelulaFormatada(7)} │ {CelulaFormatada(8)}");
            Console.WriteLine();
        }

        private string CelulaFormatada(int index)
        {
            char c = _tabuleiro[index];
            if (c == 'X') return "X";
            if (c == 'O') return "O";
            return c.ToString();
        }

        private string NomeDificuldade(NivelDificuldade nivel) => nivel switch
        {
            NivelDificuldade.Facil => "Fácil",
            NivelDificuldade.Medio => "Médio",
            NivelDificuldade.Dificil => "Difícil",
            _ => ""
        };

        private bool PedirJogada(string nomeJogador)
        {
            Console.Write($"  {nomeJogador}, escolha uma posição (1-9): ");
            string? input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out int posicao) || posicao < 1 || posicao > 9)
            {
                Console.WriteLine("  Posição inválida! Digite um número entre 1 e 9.");
                System.Threading.Thread.Sleep(DelayMensagemErroMs);
                return false;
            }

            int index = posicao - 1;
            if (_tabuleiro[index] == 'X' || _tabuleiro[index] == 'O')
            {
                Console.WriteLine("  Posição já ocupada! Escolha outra.");
                System.Threading.Thread.Sleep(DelayMensagemErroMs);
                return false;
            }

            _tabuleiro[index] = _jogadorAtual;
            return true;
        }

        private void JogadaMaquina()
        {
            switch (_nivel)
            {
                case NivelDificuldade.Facil:
                    JogadaAleatoria();
                    break;
                case NivelDificuldade.Medio:
                    // 60% chance de jogar estrategicamente
                    if (_random.NextDouble() < ProbabilidadeEstrategicaMedio)
                        JogadaEstrategica();
                    else
                        JogadaAleatoria();
                    break;
                case NivelDificuldade.Dificil:
                    JogadaOtima();
                    break;
            }
        }

        private void JogadaAleatoria()
        {
            var livres = PosicoesDisponiveis();
            int escolha = livres[_random.Next(livres.Count)];
            _tabuleiro[escolha] = _jogadorAtual;
        }

        private void JogadaEstrategica()
        {
            // Tenta vencer
            int? jogada = EncontrarJogadaVencedora('O');
            if (jogada.HasValue) { _tabuleiro[jogada.Value] = 'O'; return; }

            // Bloqueia o jogador
            jogada = EncontrarJogadaVencedora('X');
            if (jogada.HasValue) { _tabuleiro[jogada.Value] = 'O'; return; }

            // Senão, joga aleatoriamente
            JogadaAleatoria();
        }

        private void JogadaOtima()
        {
            int melhorPontuacao = int.MinValue;
            int melhorJogada = -1;

            foreach (int i in PosicoesDisponiveis())
            {
                _tabuleiro[i] = 'O';
                int pontuacao = Minimax(_tabuleiro, false);
                _tabuleiro[i] = (char)('1' + i);
                if (pontuacao > melhorPontuacao)
                {
                    melhorPontuacao = pontuacao;
                    melhorJogada = i;
                }
            }

            _tabuleiro[melhorJogada] = 'O';
        }

        private int Minimax(char[] tabuleiro, bool isMaximizing)
        {
            if (VerificarVitoriaChar('O', tabuleiro)) return 10;
            if (VerificarVitoriaChar('X', tabuleiro)) return -10;
            if (PosicoesDisponiveis(tabuleiro).Count == 0) return 0;

            if (isMaximizing)
            {
                int melhor = int.MinValue;
                foreach (int i in PosicoesDisponiveis(tabuleiro))
                {
                    tabuleiro[i] = 'O';
                    melhor = Math.Max(melhor, Minimax(tabuleiro, false));
                    tabuleiro[i] = (char)('1' + i);
                }
                return melhor;
            }
            else
            {
                int melhor = int.MaxValue;
                foreach (int i in PosicoesDisponiveis(tabuleiro))
                {
                    tabuleiro[i] = 'X';
                    melhor = Math.Min(melhor, Minimax(tabuleiro, true));
                    tabuleiro[i] = (char)('1' + i);
                }
                return melhor;
            }
        }

        private int? EncontrarJogadaVencedora(char simbolo)
        {
            foreach (int i in PosicoesDisponiveis())
            {
                _tabuleiro[i] = simbolo;
                bool vence = VerificarVitoriaChar(simbolo, _tabuleiro);
                _tabuleiro[i] = (char)('1' + i);
                if (vence) return i;
            }
            return null;
        }

        private System.Collections.Generic.List<int> PosicoesDisponiveis()
            => PosicoesDisponiveis(_tabuleiro);

        private System.Collections.Generic.List<int> PosicoesDisponiveis(char[] tabuleiro)
        {
            var livres = new System.Collections.Generic.List<int>();
            for (int i = 0; i < 9; i++)
                if (tabuleiro[i] != 'X' && tabuleiro[i] != 'O')
                    livres.Add(i);
            return livres;
        }

        private bool VerificarVitoria(char simbolo) => VerificarVitoriaChar(simbolo, _tabuleiro);

        private bool VerificarVitoriaChar(char simbolo, char[] t)
        {
            int[][] linhasVitoria =
            {
                new[] {0,1,2}, new[] {3,4,5}, new[] {6,7,8}, // linhas
                new[] {0,3,6}, new[] {1,4,7}, new[] {2,5,8}, // colunas
                new[] {0,4,8}, new[] {2,4,6}                  // diagonais
            };

            foreach (var linha in linhasVitoria)
                if (t[linha[0]] == simbolo && t[linha[1]] == simbolo && t[linha[2]] == simbolo)
                    return true;

            return false;
        }

        private bool VerificarEmpate() => PosicoesDisponiveis().Count == 0;

        private void AlternarJogador() => _jogadorAtual = _jogadorAtual == 'X' ? 'O' : 'X';
    }
}
