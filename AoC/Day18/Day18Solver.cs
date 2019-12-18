using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using static AoC.Logging;

namespace AoC.Day18
{
    public class Day18Solver : SolverReadAllText
    {
        private Stopwatch stopwatch;
        private TimeSpan? lastLogTime;

        public Day18Solver()
        {
            stopwatch = Stopwatch.StartNew();
            lastLogTime = null;
        }

        public override long? SolvePart1(string input)
        {
            // For each state:
            // Find the next paths to get a key
            //   > do this by brute force exploring, keeping any path that leads to a key, and rejecting any path that doesn't lead to a key
            //   > use the exploration algorithm from Day 16, important part is back tracking, so we don't cover the same paths or end in a loop

            var (initialGrid, initialPosition) = GridParser.Parse(input);

            var numberOfStepsToCollectAllKeys = new List<int>();
            var alreadyExploring = new HashSet<(Vector position, int numberOfSteps, string keysRemaining)>();
            var minNumberOfStepsToCollectAllKeys = int.MaxValue;

            Explore(
                initialGrid,
                initialPosition,
                0,
                initialGrid.NumberOfKeysRemaining,
                numberOfStepsToCollectAllKeys,
                ref minNumberOfStepsToCollectAllKeys,
                alreadyExploring);

            return numberOfStepsToCollectAllKeys.Min();
        }

        private void Explore(Grid grid,
            Vector position,
            int numberOfSteps,
            int numberOfKeysToFind,
            List<int> numberOfStepsToCollectAllKeys,
            ref int minNumberOfStepsToCollectAllKeys,
            HashSet<(Vector position, int numberOfSteps, string keysRemaining)> alreadyExploring)
        {
            var keysRemaining = grid.KeysRemaining;
            var explorerId = (position, numberOfSteps, keysRemaining);
            if (alreadyExploring.Contains(explorerId))
            {
                Logger.Information("Exploration skipped: " + new { explorerId.position, explorerId.numberOfSteps, keysRemaining, numKeysRemaining = explorerId.keysRemaining.Length });
                return;
            }
            alreadyExploring.Add(explorerId);

            if (lastLogTime == null || (stopwatch.Elapsed - lastLogTime.Value).TotalSeconds > 10)
            {
                var info = new
                {
                    numTimesAllKeysFound = numberOfStepsToCollectAllKeys.Count,
                    numberOfKeysToFind,
                    alreadyExploringCount = alreadyExploring.Count,
                    stopwatch.Elapsed,
                    minNumberOfStepsToCollectAllKeys
                };
                Logger.Debug($"State: {info}");
                lastLogTime = stopwatch.Elapsed;
            }

            if (grid.NumberOfKeysRemaining <= 4)
            {
                Logger.Debug($"Explore: {new { position, numberOfSteps, numberOfKeysToFind, grid.NumberOfKeysRemaining, keysRemaining }}");
            }

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

                if (childGrid.NumberOfKeysRemaining == 0)
                {
                    Logger.Information("numberOfStepsToCollectAllKeys found: " + keyFound.numberOfSteps);
                    numberOfStepsToCollectAllKeys.Add(keyFound.numberOfSteps);
                    minNumberOfStepsToCollectAllKeys = Math.Min(minNumberOfStepsToCollectAllKeys, keyFound.numberOfSteps);
                }
                else
                {
                    Explore(
                        childGrid,
                        keyFound.location,
                        keyFound.numberOfSteps,
                        numberOfKeysToFind,
                        numberOfStepsToCollectAllKeys,
                        ref minNumberOfStepsToCollectAllKeys,
                        alreadyExploring);
                }
            }
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
