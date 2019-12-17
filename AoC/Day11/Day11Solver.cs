using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.IntCodes;

namespace AoC.Day11
{
    public class Day11Solver : Solver<string, long?, string?>
    {
        public Day11Solver() : base(new InputLoaderReadAllText(11))
        {
        }

        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string inputProgram)
        {
            var robot = new Robot();

            intCodeComputer.ParseAndEvaluate(
                inputProgram,
                robot.GetCurrentPanelColor,
                robot.ProcessNextCommand);

            return robot.PaintedGrid.Count;
        }

        public override string? SolvePart2(string inputProgram)
        {
            var robot = new Robot();
            robot.PaintedGrid[robot.Location] = Robot.White; // The starting location is ALREADY painted white.

            intCodeComputer.ParseAndEvaluate(
                inputProgram,
                robot.GetCurrentPanelColor,
                robot.ProcessNextCommand);

            return $"Registration identifier:{Environment.NewLine}{RenderGrid(robot.PaintedGrid)}";
        }

        private static string RenderGrid(Dictionary<Vector, long> paintedGrid)
        {
            var topLeft = new Vector(
                paintedGrid.Min(location => location.Key.X),
                paintedGrid.Min(location => location.Key.Y));

            var bottomRight = new Vector(
                paintedGrid.Max(location => location.Key.X) + 1,
                paintedGrid.Max(location => location.Key.Y) + 1);

            var size = bottomRight - topLeft;
            var width = size.X;
            var height = size.Y;

            // Note, index 1 is which line, i.e. Y. Index 2 is which column, i.e. X
            var buffer = Enumerable
                .Range(0, height)
                .Select(_ => new string(' ', width).ToCharArray())
                .ToArray();

            foreach (var (location, paintColor) in paintedGrid)
            {
                var paintChar = paintColor == Robot.White ? '█' : ' ';
                var relLocation = location - topLeft;
                buffer[relLocation.Y][relLocation.X] = paintChar;
            }

            return string.Join(
                Environment.NewLine,
                buffer.Select(chars => new string(chars.ToArray())));
        }
    }
}
