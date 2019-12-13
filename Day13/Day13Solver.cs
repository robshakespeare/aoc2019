using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            game.RenderInitial(true);

            return game.InitialTiles.Count(tile => tile.type == TileType.Block);
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

            long GetPlayerInput() =>
                Console.ReadKey(true).Key switch
                    {
                    ConsoleKey.LeftArrow => -1,
                    ConsoleKey.RightArrow => 1,
                    _ => 0
                    };

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
                        game.RenderPixel(outputs);
                    }

                    outputs.Clear();
                }
            }

            intCodeComputer.ParseAndEvaluateWithSignalling(EnableFreePlay(input), GetPlayerInput, DisplayComputerOutput);

            Console.SetCursorPosition(0, game.Height + 2);
            Console.Write("Final Score: ");
            ColorConsole.WriteLine(score, ConsoleColor.Green);
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
