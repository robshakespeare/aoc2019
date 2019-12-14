using System.Linq;
using Common;
using Common.IntCodes;

namespace AoC.Day2
{
    public class Day2Solver : SolverReadAllText
    {
        private static readonly IntCodeComputer IntCodeComputer = new IntCodeComputer();

        private static IntCodeState SeedProgram(string inputProgram, int noun, int verb)
        {
            var intCodeState = IntCodeComputer.Parse(inputProgram);
            intCodeState[1] = noun;
            intCodeState[2] = verb;
            return intCodeState;
        }

        public override long? SolvePart1(string inputProgram)
        {
            var intCodeState = IntCodeComputer.Evaluate(SeedProgram(inputProgram, 12, 2));
            return intCodeState[0];
        }

        public override long? SolvePart2(string inputProgram) =>
        (
            from noun in Enumerable.Range(0, 100)
            from verb in Enumerable.Range(0, 100)
            where IntCodeComputer.Evaluate(SeedProgram(inputProgram, noun, verb))[0] == 19690720
            select 100 * noun + verb
        ).Single();
    }
}
