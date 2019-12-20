using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day20.Part2
{
    /// <remarks>
    /// When you enter the maze, you are at the outermost level;
    /// when at the outermost level, only the outer labels AA and ZZ function (as the start and end, respectively);
    /// all other outer labeled tiles are effectively walls.
    /// At any other level, AA and ZZ count as walls, but the other outer labeled
    /// tiles bring you one level outward.
    ///
    /// Start/end level is zero, deeper levels are above zero.
    /// </remarks>
    public class Part2Explorer
    {
        public const int OutermostLevel = 0;

        private readonly Grid grid;
        private readonly Vector initialPosition;
        private readonly int initialNumberOfSteps;

        // KEY is grid location and level, VALUE is the number of steps from origins
        private readonly Dictionary<(Vector location, int level, string route), int> gridSteps =
            new Dictionary<(Vector location, int level, string route), int>();

        public Part2Explorer(Grid grid)
        {
            this.grid = grid;
            initialPosition = grid.StartTile;
            initialNumberOfSteps = 0;
        }

        public (int? numberOfSteps, string route)? Explore()
        {
            IReadOnlyList<(Vector edge, int level, string route)> edges = new[] {(initialPosition, OutermostLevel, "AA")}.ToReadOnlyArray();
            gridSteps[(initialPosition, OutermostLevel, "AA")] = initialNumberOfSteps;
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
                    if (grid.IsEndTile(edge.edge) && edge.level == OutermostLevel)
                    {
                        return (numberOfSteps, edge.route);
                    }
                }

                edges = newEdges;
            }

            return null;
        }

        /// <summary>
        /// Move north/east/south/west, to any explorable space, which hasn't yet been explored.
        /// If any current edge is a warp, then we warp through instead of bubbling out.
        /// </summary>
        private IReadOnlyList<(Vector edge, int level, string route)> GetNextEdges(IEnumerable<(Vector edge, int level, string route)> edges) =>
            edges
                .SelectMany(x =>
                {
                    var lastPortal = Warp.GetLastPortalFromRoute(x.route);
                    return new[]
                    {
                        grid.PerformPart2WarpIfApplicable(x.edge + MovementCommand.North.MovementVector, x.level, x.route, lastPortal),
                        grid.PerformPart2WarpIfApplicable(x.edge + MovementCommand.South.MovementVector, x.level, x.route, lastPortal),
                        grid.PerformPart2WarpIfApplicable(x.edge + MovementCommand.East.MovementVector, x.level, x.route, lastPortal),
                        grid.PerformPart2WarpIfApplicable(x.edge + MovementCommand.West.MovementVector, x.level, x.route, lastPortal)
                    };
                })
                .Where(nextPos => grid.IsAvailableLocation(nextPos.postion))
                .Where(nextPos => !gridSteps.ContainsKey(nextPos))
                .ToReadOnlyArray();
    }
}
