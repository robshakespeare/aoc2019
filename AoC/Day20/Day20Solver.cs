using System.Linq;
using Common;

namespace AoC.Day20
{
    public class Day20Solver : SolverReadAllText
    {
        public override long? SolvePart1(string input)
        {
            var grid = Grid.Create(input);
            var explorer = new Explorer(grid);

            var stepCountForPaths = explorer.Explore();

            return stepCountForPaths.Min();
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
