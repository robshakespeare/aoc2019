using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = File.ReadAllLines("input.txt")
                .Select(long.Parse)
                .SelectMany(GetRequiredFuelPartsForModule)
                .Sum();

            Console.WriteLine($"Sum of the fuel requirements: {result}");
        }

        static IEnumerable<long> GetRequiredFuelPartsForModule(long mass)
        {
            var fuel = GetRequiredFuel(mass);

            while (fuel > 0)
            {
                yield return fuel;
                fuel = GetRequiredFuel(fuel);
            }
        }

        /// <summary>
        /// To find the fuel required, take its value, divide by three, round down, and subtract 2.
        /// </summary>
        /// <remarks>
        /// Note, no need for round down because long division always just takes the whole number part.</remarks>
        static long GetRequiredFuel(long value) => value / 3 - 2;
    }
}
