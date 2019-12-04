using System;
using Common;

namespace Day4
{
    public class Program
    {
        public const string Input = "138241-674034";

        static void Main(string[] args)
        {
            using var _ = new TimingBlock("Overall");

            var solver = new Solver();
            var solution = solver.HowManyDifferentPasswordsWithinRange(Input);

            Console.WriteLine($"HowManyDifferentPasswordsWithinRange: {solution}");
        }
    }
}
