using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day20.Part1
{
    public class Part1Explorer
    {
        private readonly Grid grid;
        private readonly Vector initialPosition;
        private readonly int initialNumberOfSteps;
        private readonly Dictionary<Vector, int> gridSteps = new Dictionary<Vector, int>(); // KEY is grid location, VALUE is the number of steps from origins

        public Part1Explorer(Grid grid)
        {
            this.grid = grid;
            initialPosition = grid.StartTile;
            initialNumberOfSteps = 0;
        }

        public IEnumerable<int> Explore()
        {
            var stepCountForPaths = new List<int>();

            var edges = new[] {initialPosition}.ToReadOnlyArray();
            gridSteps[initialPosition] = initialNumberOfSteps;
            var numberOfSteps = initialNumberOfSteps;

            // spread out, through available spaces, recording each number of steps to reach the exit that are found
            var stop = false;
            while (!stop)
            {
                numberOfSteps++;
                var newEdges = GetNextEdges(edges);
                stop = edges.Count == 0;

                foreach (var edge in newEdges)
                {
                    gridSteps[edge] = numberOfSteps;

                    // If reached a key, then store it
                    if (grid.IsEndTile(edge))
                    {
                        stepCountForPaths.Add(numberOfSteps);
                    }
                }

                edges = newEdges;
            }

            return stepCountForPaths;
        }

        /// <summary>
        /// Move north/east/south/west, to any explorable space, which hasn't yet been explored.
        /// If any current edge is a warp, then we warp through instead of bubbling out.
        /// </summary>
        private IReadOnlyList<Vector> GetNextEdges(IEnumerable<Vector> edges) =>
            edges
                .SelectMany(edge =>
                    new[]
                    {
                        grid.PerformPart1WarpIfApplicable(edge + MovementCommand.North.MovementVector),
                        grid.PerformPart1WarpIfApplicable(edge + MovementCommand.South.MovementVector),
                        grid.PerformPart1WarpIfApplicable(edge + MovementCommand.East.MovementVector),
                        grid.PerformPart1WarpIfApplicable(edge + MovementCommand.West.MovementVector)
                    })
                .Where(nextPos => grid.IsAvailableLocation(nextPos))
                .Where(nextPos => !gridSteps.ContainsKey(nextPos))
                .ToReadOnlyArray();
    }
}
