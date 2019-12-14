using Common;
using Common.IntCodes;

namespace Day5
{
    public class Day5Solver : SolverReadAllText
    {
        private const int AirConditionerUnitId = 1;
        private const int ThermalRadiatorControllerId = 5;

        public override long? SolvePart1(string input) => Solve(input, AirConditionerUnitId);

        public override long? SolvePart2(string input) => Solve(input, ThermalRadiatorControllerId);

        public static long? Solve(string input, int inputSystemId)
        {
            var intCodeComputer = new IntCodeComputer();

            var result = intCodeComputer.ParseAndEvaluate(
                input,
                inputSystemId);

            return result.LastOutputValue; // i.e. the Diagnostic Code
        }
    }
}
