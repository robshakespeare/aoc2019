using System;

namespace Common
{
    public abstract class SolverNoInput
    {
        public void Run()
        {
            SolvePart(1, SolvePart1);
            SolvePart(2, SolvePart2);
        }

        public virtual int? SolvePart1()
        {
            Console.WriteLine("Part 1 not yet implemented");
            return null;
        }

        public virtual int? SolvePart2()
        {
            Console.WriteLine("Part 2 not yet implemented");
            return null;
        }

        private static void SolvePart(int partNum, Func<int?> solver)
        {
            using var _ = new TimingBlock($"Part {partNum}");
            var result = solver();
            Console.Write($"Part {partNum}: ");
            ColorConsole.WriteLine(result, ConsoleColor.Green);
        }
    }
}
