using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day18
{
    public class Explorer
    {
        private readonly Part1Result part1Result;

        private readonly Grid grid;
        private readonly Dictionary<Vector, int> gridSteps = new Dictionary<Vector, int>(); // KEY is grid location, VALUE is the number of steps from origins

        public Vector[] InitialPositions { get; }
        public int InitialNumberOfSteps { get; }

        /// <remarks>
        /// Note that the `numberOfSteps` if the number of steps to reach the current position.
        /// </remarks>
        public Explorer(Grid grid, Vector[] positions, Part1Result part1Result)
        {
            this.grid = grid;
            this.part1Result = part1Result;
            InitialPositions = positions;
            InitialNumberOfSteps = 0;
        }

        public List<(char key, int numberOfSteps, Vector location, Vector[] locations)> Explore()
        {
            var keysFound = new List<(char key, int numberOfSteps, Vector location, Vector[] locations)>();

            foreach (var ip in InitialPositions.Select((initialPosition, index) => (initialPosition, index)))
            {
                var edges = new[] { ip.initialPosition };
                gridSteps[ip.initialPosition] = InitialNumberOfSteps;
                var numberOfSteps = InitialNumberOfSteps;

                // spread out, through available spaces, recording the number of iterative spreads to any keys
                var stop = false;
                while (!stop && numberOfSteps <= part1Result.MinNumberOfStepsToCollectAllKeys)
                {
                    numberOfSteps++;
                    edges = GetNextEdges(edges);
                    stop = edges.Length == 0;

                    foreach (var edge in edges)
                    {
                        gridSteps[edge] = numberOfSteps;

                        // If reached a key, then store it
                        if (grid.IsKey(edge, out var key))
                        {
                            var locations = new Vector[InitialPositions.Length];
                            Array.Copy(InitialPositions, locations, InitialPositions.Length);
                            locations[ip.index] = edge;
                            keysFound.Add((key, numberOfSteps, edge, locations)); // store key, number of steps and location
                        }
                    }
                }
            }

            return keysFound;
        }

        /// <summary>
        /// Move north/east/south/west, to any explorable space, which doesn't yet have oxygen in it.
        /// </summary>
        private Vector[] GetNextEdges(IEnumerable<Vector> edges) =>
            edges
                .SelectMany(edge => new[]
                {
                    edge + MovementCommand.North.MovementVector,
                    edge + MovementCommand.South.MovementVector,
                    edge + MovementCommand.East.MovementVector,
                    edge + MovementCommand.West.MovementVector
                })
                .Where(nextPos => grid.IsAvailableLocation(nextPos))
                .Where(nextPos => !gridSteps.ContainsKey(nextPos))
                .ToArray();
    }
}
