using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Day3
{
    public class Solver
    {
        /// <summary>
        /// Solves what is the Manhattan distance from the central port to the closest intersection.
        /// </summary>
        public int Solve(string[] wireInputs)
        {
            var wires = ParseWires(wireInputs);

            var grid = new ConcurrentDictionary<Vector, List<Wire>>();

            // Trace the wires over the grid
            foreach (var wire in wires)
            {
                var currentPosition = Vector.CentralPort;
                foreach (var movement in wire.Movements)
                {
                    currentPosition = VisitGrid(grid, wire, currentPosition, movement);
                }
            }

            // Find the intersections
            var intersections = grid.Where(element => wires.All(wire => element.Value.Contains(wire)));

            // Calculate the Manhattan Distance from each intersection to the central port
            var distances = intersections
                .Select(intersection => Vector.ManhattanDistance(intersection.Key, Vector.CentralPort))
                .OrderBy(distance => distance);

            return distances.First();
        }

        public Vector VisitGrid(ConcurrentDictionary<Vector, List<Wire>> grid, Wire wire, Vector currentPosition, Vector movement)
        {
            var movementNormal = movement.Normal;
            var destination = currentPosition + movement;

            while (!currentPosition.Equals(destination))
            {
                currentPosition += movementNormal;
                currentPosition.VisitGrid(grid, wire);
            }

            return destination;
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