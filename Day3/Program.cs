using System;
using System.IO;
using Common;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            using var _ = new TimingBlock("Overall");

            var solver = new Solver();

            var wires = File.ReadAllLines("input.txt");

            var result = solver.Solve(wires);

            Console.WriteLine($"Least number of combined steps the wires must take to reach an intersection: {result}");
        }
    }
}
