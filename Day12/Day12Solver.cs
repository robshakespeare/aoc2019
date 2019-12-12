using Common;

namespace Day12
{
    public class Day12Solver : SolverReadAllText
    {
        public static void Main() => new Day12Solver().Run();

        public override long? SolvePart1(string input)
        {
            const int numberOfSteps = 1000;

            var simulator = new Simulator(input);
            simulator.RunSimulation(numberOfSteps);

            return simulator.CalculateTotalEnergyInTheSystem();
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
