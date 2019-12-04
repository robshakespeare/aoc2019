using System;
using Common;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            using var _ = new TimingBlock("Overall");

            const string input = "138241-674034";

            var solver = new Solver();
            var solution = solver.HowManyDifferentPasswordsWithinRange(input);

            Console.WriteLine($"HowManyDifferentPasswordsWithinRange: {solution}");
        }
    }
}
