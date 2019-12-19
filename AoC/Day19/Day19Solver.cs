using System;
using System.Linq;
using Common;
using Common.Extensions;
using Common.IntCodes;

namespace AoC.Day19
{
    public class Day19Solver : SolverReadAllText
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string inputProgram)
        {
            var lines =
                Enumerable.Range(0, 50)
                    .Select(y => string.Join(
                        "",
                        Enumerable.Range(0, 50)
                            .Select(x => intCodeComputer.ParseAndEvaluate(inputProgram, x, y).LastOutputValue == 1 ? "#" : ".")))
                    .ToReadOnlyArray();

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }

            return lines.SelectMany(line => line.ToCharArray()).Count(c => c == '#');
        }

        public override long? SolvePart2(string inputProgram)
        {
            return base.SolvePart2(inputProgram);
        }
    }
}
