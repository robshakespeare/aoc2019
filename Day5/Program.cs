using System;
using Common;

namespace Day5
{
    public class Program
    {
        public static void Main()
        {
            using var _ = new TimingBlock("Overall");

            Console.WriteLine($"Diagnostic code: {Solve()}");
        }

        public static int Solve()
        {
            return new Solver().Solve();
        }
    }
}
