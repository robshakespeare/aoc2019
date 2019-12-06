using System;
using System.IO;

namespace Common
{
    public abstract class Solver : SolverNoInput
    {
        private string[] Input { get; }

        protected Solver()
        {
            Input = LoadInput();
        }

        public sealed override int? SolvePart1() => SolvePart1(Input);

        public sealed override int? SolvePart2() => SolvePart2(Input);

        public virtual int? SolvePart1(string[] input)
        {
            Console.WriteLine("Part 1 not yet implemented");
            return null;
        }

        public virtual int? SolvePart2(string[] input)
        {
            Console.WriteLine("Part 2 not yet implemented");
            return null;
        }

        private static string[] LoadInput()
        {
            using var _ = new TimingBlock("Load input");
            return File.ReadAllLines("input.txt");
        }
    }
}
