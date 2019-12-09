using System;
using Common;
using Common.IntCodes;

namespace Day9
{
    public class Day9Solver : SolverReadAllText
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string inputProgram)
        {
            var result = intCodeComputer.ParseAndEvaluate(inputProgram, 1);

            Console.WriteLine(string.Join(",", result.Outputs));

            return result.LastOutputValue;
        }

        public override long? SolvePart2(string inputProgram)
        {
            var result = intCodeComputer.ParseAndEvaluate(inputProgram, 2);

            Console.WriteLine(string.Join(",", result.Outputs));

            return result.LastOutputValue;
        }

        ////{
        ////    return base.SolvePart2(input);
        ////}
    }
}
