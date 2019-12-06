using System;
using System.IO;
using Common;

namespace Day6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // rs-todo: time stream via lines, vs read all lines

            var solver = new Solver();

            SolvePart1(solver);
            SolvePart2(solver);
        }

        public static int SolvePart1(Solver solver)
        {
            using var _ = new TimingBlock("Part 1");
            var result = solver.WhatIsTheTotalNumberOfDirectAndIndirectOrbits();
            Console.WriteLine($"Part 1: {result}");
            return result;
        }

        public static int SolvePart2(Solver solver)
        {
            using var _ = new TimingBlock("Part 2");
            var result = solver.SolvePart2(File.ReadAllLines("input.txt"));
            Console.WriteLine($"Part 2: {result}");
            return result;
        }
    }
}
