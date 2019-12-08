using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Day3
{
    public class Day3Solver : SolverReadAllLines
    {
        /// <summary>
        /// Solves: what is the least number of combined steps the wires must take to reach an intersection.
        /// </summary>
        public override int? SolvePart2(string[] wireInputs)
        {
            Console.WriteLine("Solve Part 2 started");

            var wires = ParseWires(wireInputs);

            var grid = new Dictionary<Vector, List<(Wire wire, int numberOfSteps)>>();

            // Trace the wires over the grid
            using (new TimingBlock("Trace wires"))
            {
                foreach (var wire in wires)
                {
                    var currentPosition = Vector.CentralPort;
                    var currentNumberOfSteps = 0;
                    foreach (var movement in wire.Movements)
                    {
                        (currentPosition, currentNumberOfSteps) = VisitGrid(grid, wire, currentPosition, currentNumberOfSteps, movement);
                    }
                }
            }

            // Find the intersections
            using (new TimingBlock("Find shortest intersection"))
            {
                var intersections =
                    grid.Where(element => wires.All(wire => element.Value.Any(x => x.wire.Equals(wire))));

                // Calculate the combined steps the wires must take to reach an intersection
                var numberOfStepsList = intersections
                    .Select(intersection => intersection.Value.Sum(x => x.numberOfSteps))
                    .OrderBy(numberOfSteps => numberOfSteps);

                return numberOfStepsList.First();
            }
        }

        public (Vector currentPosition, int currentNumberOfSteps) VisitGrid(Dictionary<Vector, List<(Wire wire, int numberOfSteps)>> grid, Wire wire, Vector currentPosition, int currentNumberOfSteps, Vector movement)
        {
            var movementNormal = movement.Normal;
            var movementNormalLength = movementNormal.Length;
            var destination = currentPosition + movement;

            while (!currentPosition.Equals(destination))
            {
                currentPosition += movementNormal;
                currentNumberOfSteps += movementNormalLength;
                currentPosition.VisitGrid(grid, wire, currentNumberOfSteps);
            }

            return (destination, currentNumberOfSteps);
        }

        public Wire[] ParseWires(string[] wires) => wires
            .Select(wire => wire.Split(','))
            .Select(wireCoordinates => new Wire(wireCoordinates.Select(ParseWireCoordinate)))
            .ToArray();

        public Vector ParseWireCoordinate(string wireCoordinate)
        {
            var direction = wireCoordinate[0];

            var amountSpan = new Span<char>(wireCoordinate.ToCharArray(), 1, wireCoordinate.Length - 1);
            var amount = int.Parse(amountSpan);

            return direction switch {
                'R' => new Vector(+amount, 0),
                'L' => new Vector(-amount, 0),
                'U' => new Vector(0, +amount),
                'D' => new Vector(0, -amount),
                _ => throw new InvalidOperationException("Invalid direction: " + direction)
                };
        }
    }
}
