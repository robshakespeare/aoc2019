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

        public override long? SolvePart1(string intCodeProgram) => Solve(intCodeProgram, @"
// jump if next tile is a hole:
NOT A J

// jump if 3rd tile in front is a hole:
NOT C T
OR T J

// only jump if we have a landing:
AND D J
WALK");

        public override long? SolvePart2(string intCodeProgram) => Solve(intCodeProgram, @"
// jump if next tile is a hole:
NOT A J

// jump if 3rd tile in front is a hole:
NOT C T
OR T J

// jump if 5th is hole, as long as we have landing, and a landing after that for if we need to jump:
//NOT E T
//AND F T
//AND H T
//AND I T
//OR T J
//
// only jump if we have a landing:
AND D J
RUN");

        
        private long? Solve(string intCodeProgram, string springScript)
        {
            var inputs = new Queue<long>(Encode(springScript));

            var intCodeState = IntCodeComputer.ParseAndEvaluate(
                intCodeProgram,
                () => inputs.Dequeue(),
                output => Console.Write(output < 128 ? Encoding.ASCII.GetString(new[] {(byte) output}) : ""));

            return intCodeState.LastOutputValue;
        }

        private static long[] Encode(string s) =>
            StripComments.Replace(
                    s.Trim().NormalizeLineEndings("\n"), "")
                .Replace("\n\n", "\n")
                .ToCharArray()
                .Select(c => (long)c)
                .Append(10)
                .ToArray();

        private static readonly Regex StripComments = new Regex(@" ?//.*\n", RegexOptions.Compiled);
    }
}
