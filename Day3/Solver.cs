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
                foreach (var coordinate in wire.Coordinates)
                {
                    coordinate.VisitGrid(grid, wire);
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

        public Wire[] ParseWires(string[] wires) => wires
            .Select(wire => wire.Split(','))
            .Select(wireCoordinates =>
            {
                var currentPosition = Vector.CentralPort;

                var path = wireCoordinates.Select(wireCoordinate =>
                {
                    var coordinate = ParseWireCoordinate(wireCoordinate);
                    currentPosition += coordinate;
                    return currentPosition;
                }).ToArray();

                return new Wire(path);
            })
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