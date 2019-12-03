using System;
using System.IO;
using Common;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            using (new TimingBlock())
            {
                var solver = new Solver();

                var wires = File.ReadAllLines("input.txt");

                var result = solver.Solve(wires);

                Console.WriteLine($"Manhattan distance from the central port to the closest intersection: {result}");
            }
        }
    }
}
