using Common;
using Common.IntCodes;

namespace Day9
{
    public class Day9Solver : SolverReadAllText
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string inputProgram) => intCodeComputer.ParseAndEvaluate(inputProgram, 1).LastOutputValue;

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
