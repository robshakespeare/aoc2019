using System;
using System.IO;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver();

            var wires = File.ReadAllLines("input.txt");

            var result = solver.Solve(wires);

            Console.WriteLine($"Manhattan distance from the central port to the closest intersection: {result}");
        }
    }
}
