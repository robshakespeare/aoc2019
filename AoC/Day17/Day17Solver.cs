using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Extensions;
using Common.IntCodes;

namespace AoC.Day17
{
    public class Day17Solver : SolverReadAllText
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        private static readonly Vector NorthNormal = new Vector(0, -1);
        private static readonly Vector SouthNormal = new Vector(0, 1);
        private static readonly Vector EastNormal = new Vector(1, 0);
        private static readonly Vector WestNormal = new Vector(-1, 0);

        public override long? SolvePart1(string inputProgram)
        {
            var lines = new List<StringBuilder>();
            var grid = new Dictionary<Vector, char>();
            var lineBuffer = new StringBuilder();

            void HandleOutput(long outputValue)
            {
                if (outputValue == 10)
                {
                    // new line
                    lines.Add(lineBuffer);
                    lineBuffer = new StringBuilder();
                }
                else
                {
                    var outputChar = (char) outputValue;
                    grid.Add(new Vector(lineBuffer.Length, lines.Count), outputChar);
                    lineBuffer.Append(outputChar);
                }
            }

            intCodeComputer.ParseAndEvaluateWithSignalling(inputProgram, () => 0, HandleOutput);

            var intersections = grid
                .Where(p => p.Value == '#')
                .Where(p => new[] {p.Key + NorthNormal, p.Key + SouthNormal, p.Key + EastNormal, p.Key + WestNormal}
                                .Select(s => grid.TryGetValue(s, out var sc) && sc == '#')
                                .Count(hasScaffold => hasScaffold) > 2)
                .ToReadOnlyArray();

            foreach (var intersection in intersections)
            {
                lines[intersection.Key.Y][intersection.Key.X] = 'O';
            }

            DisplayLines(lines);

            return intersections.Sum(intersection => intersection.Key.X * intersection.Key.Y);
        }

        private static void DisplayLines(IEnumerable<StringBuilder> lines)
        {
            foreach (var line in lines)
            {
                Console.WriteLine(line.ToString());
            }
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
