using System;
using System.Collections.Generic;
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
            var intCodeState = intCodeComputer.Parse(inputProgram);

            var lines =
                Enumerable.Range(0, 50)
                    .Select(y => string.Join(
                        "",
                        Enumerable.Range(0, 50)
                            .Select(x => intCodeComputer.Evaluate(intCodeState.CloneWithReset(), x, y).LastOutputValue == 1 ? "#" : ".")))
                    .ToReadOnlyArray();

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }

            return lines.SelectMany(line => line.ToCharArray()).Count(c => c == '#');
        }

        private const int RequiredSizePuzzle = 100;

        public override long? SolvePart2(string inputProgram) => SolvePart2(inputProgram, YieldLine, RequiredSizePuzzle);

        public long? SolvePart2(string inputProgram, Func<IntCodeState, int, int, IEnumerable<string>> yieldLine, int requiredSize)
        {
            var searchForString = new string(Enumerable.Repeat('#', requiredSize).ToArray());

            var intCodeState = intCodeComputer.Parse(inputProgram);

            var lines = new List<string>();
            var candidateLines = new List<(string line, int y)>();
            string? line = null;
            long? result = null;

            foreach (var y in Enumerable.Range(0, int.MaxValue))
            {
                var x = 0;
                if (line != null)
                {
                    x = line.Select((c, i) => (c, i)).FirstOrDefault(p => p.c == '#').i;
                }

                line = string.Join("", yieldLine(intCodeState, x, y));

                if (line.Count(c => c == '#') > requiredSize)
                {
                    candidateLines.Add((line, y));

                    // We can only have had our match, once there is at least that number of candidate lines!
                    if (candidateLines.Count >= requiredSize)
                    {
                        result = FindMatch(candidateLines, searchForString, requiredSize);

                        if (result != null)
                        {
                            break;
                        }
                    }
                }

                lines.Add(line);
            }

            foreach (var printLine in lines)
            {
                Console.WriteLine(printLine);
            }

            return result;
        }

        private static long? FindMatch(List<(string line, int y)> candidateLines, string searchForString, int requiredSize)
        {
            // Do the check, go back by the required number of lines
            // If that ones last index, matches our first index, then we have our required space!

            for (var candidateTopIndex = 0; candidateTopIndex < candidateLines.Count; candidateTopIndex++)
            {
                var candidateBottomIndex = candidateTopIndex + (requiredSize - 1);
                if (candidateBottomIndex < candidateLines.Count)
                {
                    var topLine = candidateLines[candidateTopIndex];
                    var bottomLine = candidateLines[candidateBottomIndex];

                    var topLineLeftIndex = topLine.line.LastIndexOf(searchForString, StringComparison.OrdinalIgnoreCase);
                    var bottomLineLeftIndex = bottomLine.line.IndexOf(searchForString, StringComparison.OrdinalIgnoreCase);

                    if (topLineLeftIndex == bottomLineLeftIndex)
                    {
                        // What value do you get if you take that point's X coordinate, multiply it by 10000, then add the point's Y coordinate?
                        return (topLineLeftIndex * 10000) + topLine.y;
                    }
                }
            }

            return null;
        }

        private IEnumerable<string> YieldLine(IntCodeState intCodeState, int x, int y)
        {
            for (var p = 0; p < x; p++)
            {
                yield return ".";
            }

            var hadHash = false;
            var hadDotFollowingHash = false;
            while (!hadDotFollowingHash)
            {
                var res = intCodeComputer.Evaluate(intCodeState.CloneWithReset(), x, y).LastOutputValue == 1 ? "#" : ".";

                if (res == "#")
                {
                    hadHash = true;
                }
                else if (hadHash && res == ".")
                {
                    hadDotFollowingHash = true;
                }

                yield return res;
                x++;

                if (y < 3 && x > 5)
                {
                    break;
                }
            }
        }
    }
}
