using System;
using System.IO;
using Common;

namespace Day5
{
    public class Program
    {
        const int airConditionerUnitId = 1;
        const int thermalRadiatorControllerId = 5;

        static Lazy<string> Input = new Lazy<string>(() => File.ReadAllText("input.txt"));

        public static void Main()
        {
            Console.WriteLine($"Part 1 - Diagnostic code: {SolvePart1()}");

            Console.WriteLine($"Part 2 - Diagnostic code: {SolvePart2()}");
        }

        public static int? SolvePart1() => Solve(airConditionerUnitId);

        public static int? SolvePart2() => Solve(thermalRadiatorControllerId);

        public static int? Solve(int inputSystemId)
        {
            using var _ = new TimingBlock($"Solve {new { inputSystemId }}");

            var intCodeComputer = new IntCodeComputer();

            var result = intCodeComputer.ParseAndEvaluate(
                Input.Value,
                inputSystemId);

            return result.diagnosticCode;
        }
    }
}
