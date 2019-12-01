using System;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = File.ReadAllLines("input.txt")
                .Select(int.Parse)
                .Select(GetRequiredFuelForModule)
                .Sum();

            Console.WriteLine($"Sum of the fuel requirements: {result}");
        }

        /// <summary>
        /// To find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.
        /// </summary>
        /// <remarks>
        /// Note, no need for round down because int division always just takes the whole number part.</remarks>
        public static int GetRequiredFuelForModule(int mass) => mass / 3 - 2;
    }
}
