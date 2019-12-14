using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Common.IntCodes;
using MoreLinq;

namespace AoC.Day13
{
    /// <summary>
    /// Every three output instructions specify
    /// 0) x position (distance from the left)
    /// 1) y position (distance from the top)
    /// 2) tile type
    ///
    /// For our input:
    /// Min: X=0, Y=0
    /// Max: X=43, Y=23
    /// </summary>
    public class Day13Solver : SolverReadAllText
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string input)
        {
            var game = InitializeGame(input);

            RenderInitial(game);

            return game.InitialTiles.Count(tile => tile.type == TileType.Block);
        }

        public void RenderInitial(Game game)
        {
            Console.WriteLine($"TopLeft: X={game.Left}, Y={game.Top}");
            Console.WriteLine($"BottomRight: X={game.Right}, Y={game.Bottom}");

            // Index 1 is which line, i.e. Y.
            // Index 2 is which column, i.e. X.
            var buffer = Enumerable
                .Range(0, game.Height)
                .Select(_ => new string(' ', game.Width).ToCharArray())
                .ToArray();

            foreach (var (pos, type) in game.InitialTiles)
            {
                var paintChar = Game.GetPaintChar(type);
                var relLocation = pos - game.TopLeft;
                buffer[relLocation.Y][relLocation.X] = paintChar.paintChar;
            }

            var grid = string.Join(
                Environment.NewLine,
                buffer.Select(chars => new string(chars.ToArray())));

            Console.WriteLine(grid);
        }

        private Game InitializeGame(string input)
        {
            var result = intCodeComputer.ParseAndEvaluate(input);

            var tiles = result.Outputs
                .Batch(3)
                .Select(batch => Game.ParseOutputBatch(batch.ToArray()))
                .ToArray();

            return new Game(tiles);
        }

        public void Play()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();

            Console.SetCursorPosition(7, 10);
            Console.Write("Loading...");

            var input = new InputLoaderReadAllText(13).LoadInput();
            Console.Clear();

            var auto = false;
            var game = InitializeGame(input);
            var score = 0L;
            var abandon = false;

            Console.CancelKeyPress += (sender, e) =>
            {
                score = 0;
                abandon = true;
                e.Cancel = true;
            };

            long GetAIPlayerInput()
            {
                if (Console.KeyAvailable)
                {
                    var readKey = Console.ReadKey(true);

                    if (readKey.Key == ConsoleKey.Q)
                    {
                        score = 0;
                        abandon = true;
                        return 0;
                    }

                    // Switch off auto solver!
                    if (auto && readKey.Key == ConsoleKey.S)
                    {
                        auto = false;
                    }
                }

                if (game.PaddlePosition.X < game.BallPosition.X)
                {
                    return 1;
                }

                if (game.PaddlePosition.X > game.BallPosition.X)
                {
                    return -1;
                }

                return 0;
            }

            var started = false;
            var clearPrompt = false;

            long GetPlayerInput()
            {
                if (!started)
                {
                    started = true;

                    Write(game.Right + 2, 3, "Move", "Left / Right keys");
                    Write(game.Right + 2, 4, "Stay still", "Any other key");
                    Write(game.Right + 2, 5, "Auto move", "A");
                    Write(game.Right + 2, 6, "Auto solve", "S");
                    Write(game.Right + 2, 7, "Quit", "Q / Ctrl+C");

                    Console.SetCursorPosition(game.Right + 2, 9);
                    Console.Write("Press a key to begin");
                    clearPrompt = true;
                }

                while (!Console.KeyAvailable && !abandon)
                {
                    Thread.Sleep(10);
                }

                if (clearPrompt)
                {
                    Console.SetCursorPosition(game.Right + 2, 9);
                    Console.Write("".PadRight(20));
                }

                if (abandon)
                {
                    return 0;
                }

                var readKey = Console.ReadKey(true);

                if (readKey.Key == ConsoleKey.Q)
                {
                    score = 0;
                    abandon = true;
                    return 0;
                }

                // Auto move!
                if (readKey.Key == ConsoleKey.A)
                {
                    return GetAIPlayerInput();
                }

                // Switch to auto solver!
                if (readKey.Key == ConsoleKey.S)
                {
                    auto = true;
                    return GetAIPlayerInput();
                }

                return readKey.Key switch
                    {
                    ConsoleKey.LeftArrow => -1,
                    ConsoleKey.RightArrow => 1,
                    _ => 0
                    };
            }

            var outputs = new List<long>();

            void DisplayOutput(long value)
            {
                outputs.Add(value);

                if (outputs.Count == 3)
                {
                    if (outputs[0] == -1 && outputs[1] == 0)
                    {
                        score = outputs[2];
                        Write(game.Right + 2, 1, "Score", score.ToString().PadRight(20));
                    }
                    else
                    {
                        game.Update(outputs);
                    }

                    outputs.Clear();
                }
            }

            var intCodeState = intCodeComputer.Parse(EnableFreePlay(input));

            while (!abandon &&
                   intCodeComputer.EvaluateNextInstruction(
                       intCodeState,
                       auto ? (Func<long>) GetAIPlayerInput : GetPlayerInput,
                       DisplayOutput))
            {
            }

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write("Final Score: ");
            ColorConsole.WriteLine(score.ToString(), ConsoleColor.Green);
            Console.CursorVisible = true;
        }

        private static void Write(int x, int y, string text, string colorText)
        {
            Console.SetCursorPosition(x, y);
            Console.Write((text + ':').PadRight(12));
            ColorConsole.Write(colorText, ConsoleColor.Yellow);
        }

        private static string EnableFreePlay(string input)
        {
            var chars = input.ToCharArray();
            chars[0] = '2';
            return new string(chars);
        }
    }
}
