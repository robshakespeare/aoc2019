using System;
using Common;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            using var _ = new TimingBlock("Overall");

            var solution = new Solver().Solve();
            Console.WriteLine($"Solution: {solution}");
        }
    }
}
