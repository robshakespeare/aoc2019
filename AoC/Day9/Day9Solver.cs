using System;
using Common;
using Common.IntCodes;

namespace AoC.Day9
{
    public class Day9Solver : SolverReadAllText
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string inputProgram) => Solve(inputProgram, 1);

        public override long? SolvePart2(string inputProgram) => Solve(inputProgram, 2);

        private long? Solve(string inputProgram, long inputValue)
        {
            var result = intCodeComputer.ParseAndEvaluate(inputProgram, inputValue);

            if (result.Outputs.Count != 1)
            {
                throw new InvalidOperationException("Expected only one output, but instead got: " + string.Join(",", result.Outputs));
            }

            return result.LastOutputValue;
        }
    }
}
