using System;

namespace AoC.Day20.Part2
{
    public static class Part2Solver
    {
        public static long? SolvePart2(string input)
        {
            var grid = Grid.Create(input);
            var explorer = new Part2Explorer(grid);

            var result = explorer.Explore();

            Console.WriteLine($"Number of steps: {result?.numberOfSteps}");
            Console.WriteLine($"Route: {result?.route}");

            return result?.numberOfSteps;
        }
    }
}
