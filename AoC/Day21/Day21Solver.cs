using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common;
using Common.Extensions;
using Common.IntCodes;

namespace AoC.Day21
{
    public class Day21Solver : SolverReadAllText
    {
        private static readonly IntCodeComputer IntCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string input)
        {
            var inputs = new Queue<long>(Encode(@"
// jump if a three-tile-wide hole (with ground on the other side of the hole) is detected:
NOT A J
NOT B T
AND T J
NOT C T
AND T J
AND D J

// jump if next tile is a hole, as long as we have a landing:
NOT A T
AND D T
OR T J

// jump if 3rd tile in front is a hole, as long as we have a landing:
NOT C T
AND D T
OR T J

WALK"));

            var intCodeState = IntCodeComputer.ParseAndEvaluate(
                input,
                () => inputs.Dequeue(),
                output => Console.Write(output < 128 ? Encoding.ASCII.GetString(new[] {(byte) output}) : ""));

            return intCodeState.LastOutputValue;
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }

        private static long[] Encode(string s) =>
            StripComments.Replace(
                    s.Trim().NormalizeLineEndings("\n"), "")
                .Replace("\n\n", "\n")
                .ToCharArray()
                .Select(c => (long)c)
                .Append(10)
                .ToArray();

        private static readonly Regex StripComments = new Regex(@" ?//.+\n", RegexOptions.Compiled);
    }
}
