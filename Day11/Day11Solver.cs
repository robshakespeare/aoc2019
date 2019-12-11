using Common;
using Common.IntCodes;

namespace Day11
{
    public class Day11Solver : SolverReadAllText
    {
        public static void Main() => new Day11Solver().Run();

        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string inputProgram)
        {
            var robot = new Robot();

            intCodeComputer.ParseAndEvaluateWithSignalling(
                inputProgram,
                robot.GetCurrentPanelColor,
                robot.ProcessNextCommand);

            return robot.PaintedGrid.Count;
        }

        public override long? SolvePart2(string inputProgram)
        {
            return base.SolvePart2(inputProgram);
        }
    }
}
