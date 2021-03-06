using Common;

namespace AoC.Day12
{
    public class Day12Solver : SolverReadAllText
    {
        public override long? SolvePart1(string input)
        {
            const int numberOfSteps = 1000;

            var simulator = new Simulator(input);
            simulator.RunSimulation(numberOfSteps);

            return simulator.CalculateTotalEnergyInTheSystem();
        }

        public override long? SolvePart2(string input)
        {
            var simulator = new Simulator(input);
            return simulator.FindFirstRepeatingStateStepNumber();
        }
    }
}
