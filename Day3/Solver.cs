using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Day3
{
    public class Solver
    {
        /// <summary>
        /// Solves: what is the least number of combined steps the wires must take to reach an intersection.
        /// </summary>
        public int Solve(string[] wireInputs)
        {
            var wires = ParseWires(wireInputs);

            var grid = new ConcurrentDictionary<Vector, List<(Wire wire, int numberOfSteps)>>();

            // Trace the wires over the grid
            foreach (var wire in wires)
            {
                var currentPosition = Vector.CentralPort;
                var currentNumberOfSteps = 0;
                foreach (var movement in wire.Movements)
                {
                    (currentPosition, currentNumberOfSteps) = VisitGrid(grid, wire, currentPosition, currentNumberOfSteps, movement);
                }
            }

            // Find the intersections
            var intersections = grid.Where(element => wires.All(wire => element.Value.Any(x => x.wire.Equals(wire))));

            // Calculate the combined steps the wires must take to reach an intersection
            var numberOfStepsList = intersections
                .Select(intersection => intersection.Value.Sum(x => x.numberOfSteps))
                .OrderBy(numberOfSteps => numberOfSteps);

            return numberOfStepsList.First();
        }

        public (Vector currentPosition, int currentNumberOfSteps) VisitGrid(ConcurrentDictionary<Vector, List<(Wire wire, int numberOfSteps)>> grid, Wire wire, Vector currentPosition, int currentNumberOfSteps, Vector movement)
        {
            var movementNormal = movement.Normal;
            var destination = currentPosition + movement;

            while (!currentPosition.Equals(destination))
            {
                currentPosition += movementNormal;
                currentNumberOfSteps += movementNormal.Length;
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
            var amount = int.Parse(new string(wireCoordinate.Skip(1).ToArray())); // rs-todo: could try using Span here

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