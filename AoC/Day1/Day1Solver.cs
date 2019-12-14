using System.Collections.Generic;
using System.Linq;
using Common;

namespace Day1
{
    public class Day1Solver : SolverReadAllLines
    {
        public override long? SolvePart1(string[] input) =>
            input.Select(int.Parse)
                .Select(GetRequiredFuel)
                .Sum();

        public override long? SolvePart2(string[] input) =>
            input.Select(int.Parse)
                .SelectMany(GetRequiredFuelPartsForModule)
                .Sum();

        private static IEnumerable<int> GetRequiredFuelPartsForModule(int mass)
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
        /// <remarks>Note, no need for round down because long division always just takes the whole number part.</remarks>
        private static int GetRequiredFuel(int value) => value / 3 - 2;
    }
}
