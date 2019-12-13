using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Common;
using Common.IntCodes;
using MoreLinq;

namespace Day13
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
        public static void Main() => new Day13Solver().Play();

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
                var paintChar = game.GetPaintChar(type);
                var relLocation = pos - game.TopLeft;
                buffer[relLocation.Y][relLocation.X] = paintChar;
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
            Console.CursorVisible = false;

            var input = File.ReadAllText("input.txt");
            var game = InitializeGame(input);

            long GetPlayerInput()
            {
                Thread.Sleep(200);

                var nextBallPosition = game.BallPosition + game.BallMovement;

                // if ball pos in the next frame is going to be where we are on the X axis, then stay still
                // otherwise, move closer to its next X pos
                ////if (nextBallPosition.X == game.PaddlePosition.X)
                ////{
                ////    return 0;
                ////}

                var delta = nextBallPosition.X - game.PaddlePosition.X;

                if (nextBallPosition.Y >= game.PaddlePosition.Y)
                {
                    return 0;
                }

                var playerInput = delta switch
                    {
                    0 => 0,
                    _ when delta == -1 && game.BallMovement.X == 1 && game.BallMovement.Y == 1 => 0,
                    _ when delta == 1 && game.BallMovement.X == -1 && game.BallMovement.Y == 1 => 0,
                    _ when delta < 0 => -1,
                    _ => 1
                    };

                Debug.WriteLine($"{new { playerInput, delta, game.BallPosition, game.BallMovement, nextBallPosition, game.PaddlePosition }}");

                return playerInput;

                ////return Console.ReadKey(true).Key switch
                ////{
                ////    ConsoleKey.LeftArrow => -1,
                ////    ConsoleKey.RightArrow => 1,
                ////    _ => 0
                ////};
            }

            var outputs = new List<long>();
            var score = 0L;

            void DisplayComputerOutput(long value)
            {
                outputs.Add(value);

                if (outputs.Count == 3)
                {
                    if (outputs[0] == -1 && outputs[1] == 0)
                    {
                        score = outputs[2];
                        Console.SetCursorPosition(game.Right + 2, 1);
                        Console.Write($"Score: {score.ToString().PadLeft(20)}");
                    }
                    else
                    {
                        game.Update(outputs);
                    }

                    outputs.Clear();
                }
            }

            intCodeComputer.ParseAndEvaluateWithSignalling(EnableFreePlay(input), GetPlayerInput, DisplayComputerOutput);

            Console.SetCursorPosition(0, game.Height + 2);
            Console.Write("Final Score: ");
            ColorConsole.WriteLine(score.ToString(), ConsoleColor.Green);
            Console.CursorVisible = true;
        }

        private static string EnableFreePlay(string input)
        {
            var chars = input.ToCharArray();
            chars[0] = '2';
            return new string(chars);
        }
    }
}
