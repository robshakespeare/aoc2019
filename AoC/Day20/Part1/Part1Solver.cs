using System.Linq;

namespace AoC.Day20.Part1
{
    public static class Part1Solver
    {
        public static long SolvePart1(string input)
        {
            var grid = Grid.Create(input);
            var explorer = new Part1Explorer(grid);

            var stepCountForPaths = explorer.Explore();

            return stepCountForPaths.Min();
        }
    }
}
