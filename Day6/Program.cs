using System;
using System.IO;
using Common;

namespace Day6
{
    public class Program
    {
        private static readonly Lazy<string[]> InputLines = new Lazy<string[]>(() => File.ReadAllLines("input.txt"));

        public static void Main()
        {
            using var _ = new TimingBlock("Overall");

            var solver = new Solver();

            ReadInput();
            SolvePart1(solver);
            SolvePart2(solver);
        }

        public static int ReadInput()
        {
            using var _ = new TimingBlock("Read input");
            return InputLines.Value.Length;
        }

        public static int SolvePart1(Solver solver)
        {
            using var _ = new TimingBlock("Part 1");
            var result = solver.SolvePart1(InputLines.Value);
            Console.WriteLine($"Part 1: {result}");
            return result;
        }

        public static int SolvePart2(Solver solver)
        {
            using var _ = new TimingBlock("Part 2");
            var result = solver.SolvePart2(InputLines.Value);
            Console.WriteLine($"Part 2: {result}");
            return result;
        }
    }
}
