using Common;
using Common.IntCodes;

namespace Day5
{
    public class Program : SolverReadAllText
    {
        private const int AirConditionerUnitId = 1;
        private const int ThermalRadiatorControllerId = 5;

        public static void Main() => new Program().Run();

        public override int? SolvePart1(string input) => Solve(input, AirConditionerUnitId);

        public override int? SolvePart2(string input) => Solve(input, ThermalRadiatorControllerId);

        public static int? Solve(string input, int inputSystemId)
        {
            var intCodeComputer = new IntCodeComputer();

            var result = intCodeComputer.ParseAndEvaluate(
                input,
                inputSystemId);

            return result.LastOutputValue; // i.e. the Diagnostic Code
        }
    }
}
