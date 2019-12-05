using System;
using System.IO;
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
            var intCodeComputer = new IntCodeComputer();

            // Note: const int airConditionerUnitId = 1;
            const int thermalRadiatorControllerId = 5;

            var result = intCodeComputer.ParseAndEvaluate(
                File.ReadAllText("input.txt"),
                thermalRadiatorControllerId);

            return result.diagnosticCode;
        }
    }
}
