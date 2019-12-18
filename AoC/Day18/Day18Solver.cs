using System.Collections.Generic;
using System.Linq;
using Common;
using static AoC.Logging;

namespace AoC.Day18
{
    public class Day18Solver : SolverReadAllText
    {
        public override long? SolvePart1(string input)
        {
            // For each state:
            // Find the next paths to get a key
            //   > do this by brute force exploring, keeping any path that leads to a key, and rejecting any path that doesn't lead to a key
            //   > use the exploration algorithm from Day 16, important part is back tracking, so we don't cover the same paths or end in a loop

            var (initialGrid, initialPosition) = GridParser.Parse(input);

            var numberOfStepsToCollectAllKeys = new List<int>();

            Explore(
                initialGrid,
                initialPosition,
                0,
                initialGrid.NumberOfKeys,
                numberOfStepsToCollectAllKeys);

            return numberOfStepsToCollectAllKeys.Min();
        }

        private static void Explore(
            Grid grid,
            Vector position,
            int numberOfSteps,
            int numberOfKeysToFind,
            List<int> numberOfStepsToCollectAllKeys)
        {
            Logger.Debug($"Explore: {new { position, numberOfSteps, numberOfKeysToFind, numberOfKeysRemaining = grid.NumberOfKeys }}");

            var explorer = new Explorer(grid, position, numberOfSteps);

            explorer.Explore();

            var keysFound = explorer.KeysFound;

            // Then, for each path, we need to repeat that, which will be exponential, but hopefully not too resource intensive
            //   > each path was to a key, before we recurse:
            //       > clone the grid
            //       > we should reset our "explored" paths
            //       > use that key to unlock its door
            //       > Note: if no paths, then there's no keys left, so store this route's number of steps, and DON'T recurse
            foreach (var keyFound in keysFound)
            {
                var childGrid = grid.Clone();
                childGrid.PickUpKeyAndUnlockDoor(keyFound.location);

                if (childGrid.NumberOfKeys == 0)
                {
                    numberOfStepsToCollectAllKeys.Add(keyFound.numberOfSteps);
                }
                else
                {
                    Explore(childGrid, keyFound.location, keyFound.numberOfSteps, numberOfKeysToFind, numberOfStepsToCollectAllKeys);
                }
            }
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
