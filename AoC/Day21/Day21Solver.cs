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
// jump if next tile (A) is a hole:
NOT A J

// jump if 2nd tile (B) in front is a hole:
NOT B T
OR T J

// jump if 3rd tile (C) in front is a hole:
NOT C T
OR T J

// only jump if we have a landing (i.e 4th tile == D):
AND D J

// only jump if 5th (E) or 8th (H) are ground
NOT J T // if we are to jump, T will now be false
OR E T
OR H T
AND T J
RUN");

////        public override long? SolvePart2(string intCodeProgram) => Solve(intCodeProgram, @"
////OR G T
////AND H T
////AND I T // by this point T == true, if there is ground at 7th, 8th and 9th

////////AND T J // by this point J == true, if there is the gap
////////NOT J J // by this point J == true, if there is NO gap

////// jump if 3rd tile in front is a hole, if it is followed by ground at 7th, 8th and 9th
////// i.e. always jump if 3rd tile is a hole, except for when we have the 3 gap at 7th, 8th and 9th
////NOT C J
////AND T J

//////AND E J

//////// jump if 2nd and 5th is hole
////////NOT B J
////////NOT E T
////////AND T J

////// jump if next tile is a hole:
////NOT A T
////OR T J

////// jump if 2nd tile is a hole:
////NOT B T
////OR T J

//////// jump if 5th is hole, as long as we have landing, and a landing after that for if we need to jump:
////////NOT E T
////////AND F T
////////AND H T
////////AND I T
////////OR T J

////// only jump if we have a landing:
////AND D J
////RUN");

        private static long? Solve(string intCodeProgram, string springScript)
        {
            var inputs = new Queue<long>(Encode(springScript));

            var intCodeState = IntCodeComputer.ParseAndEvaluate(
                intCodeProgram,
                () =>
                {
                    var input = inputs.Dequeue();
                    Console.Write((char)input);
                    return input;
                },
                output => Console.Write(output < 128 ? Encoding.ASCII.GetString(new[] {(byte) output}) : ""));

            return intCodeState.LastOutputValue;
        }

        private static IEnumerable<long> Encode(string s) =>
            StripComments.Replace(s, "")
                .NormalizeLineEndings()
                .ReadAllLines()
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .SelectMany(
                    line => line.ToCharArray()
                        .Select(c => (long) c)
                        .Append(10));

        private static readonly Regex StripComments = new Regex(@" ?//.*$", RegexOptions.Compiled | RegexOptions.Multiline);
    }
}
