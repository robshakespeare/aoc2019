using System.Collections.Generic;
using System.Linq;

namespace AoC.Day18
{
    public class Explorer
    {
        private readonly Part1Result part1Result;

        private readonly Grid grid;
        private readonly Dictionary<Vector, int> gridSteps = new Dictionary<Vector, int>(); // KEY is grid location, VALUE is the number of steps from origins

        private readonly List<(char key, int numberOfSteps, Vector location)> keysFound = new List<(char key, int numberOfSteps, Vector location)>();

        public Vector InitialPosition { get; }
        public int InitialNumberOfSteps { get; }

        /// <remarks>
        /// Note that the `numberOfSteps` if the number of steps to reach the current position.
        /// </remarks>
        public Explorer(Grid grid, Vector position, Part1Result part1Result)
        {
            this.grid = grid;
            this.part1Result = part1Result;
            InitialPosition = position;
            InitialNumberOfSteps = 0;
        }

        public List<(char key, int numberOfSteps, Vector location)> Explore()
        {
            var edges = new[] { InitialPosition };
            gridSteps[InitialPosition] = InitialNumberOfSteps;
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
                        keysFound.Add((key, numberOfSteps, edge)); // store key, number of steps and location
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
