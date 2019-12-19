using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Common;
using static AoC.Logging;

namespace AoC.Day18
{
    public class Part1Result
    {
        public int MinNumberOfStepsToCollectAllKeys { get; set; } = int.MaxValue;
    }

    public class Day18Solver : SolverReadAllText
    {
        private readonly Stopwatch stopwatch;
        private TimeSpan? lastLogTime;
        private readonly object mutex = new object();

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

            var cache = new ConcurrentDictionary<(Vector position, int numberOfSteps, string keysRemaining), char>();
            var part1Result = new Part1Result();

            Explore(
                initialGrid,
                initialPosition,
                0,
                initialGrid.NumberOfKeysRemaining,
                true,
                part1Result,
                cache);

            return part1Result.MinNumberOfStepsToCollectAllKeys;
        }

        private void Explore(Grid grid,
            Vector position,
            int numberOfSteps,
            int numberOfKeysToFind,
            bool isFirstLevel,
            Part1Result part1Result,
            ConcurrentDictionary<(Vector position, int numberOfSteps, string keysRemaining), char> cache)
        {
            if (numberOfSteps >= part1Result.MinNumberOfStepsToCollectAllKeys)
            {
                return;
            }

            var keysRemaining = grid.KeysRemaining;
            var explorerId = (position, numberOfSteps, keysRemaining);
            if (cache.TryAdd(explorerId, 'y') == false) // TryAdd returns false if the key already exists
            {
                ////Logger.Information("Exploration skipped: " + explorerId);
                return;
            }

            if (lastLogTime == null || (stopwatch.Elapsed - lastLogTime.Value).TotalSeconds > 10)
            {
                var info = new
                {
                    numberOfKeysToFind,
                    cacheCount = cache.Count,
                    stopwatch.Elapsed,
                    part1Result.MinNumberOfStepsToCollectAllKeys
                };
                Logger.Information($"State: {info}");
                lastLogTime = stopwatch.Elapsed;
            }

            var explorer = new Explorer(grid, position, numberOfSteps, part1Result);

            var keysFound = explorer.Explore(); // .OrderBy(x => x.numberOfSteps); // rs-todo: ????

            // Then, for each path, we need to repeat that, which will be exponential, but hopefully not too resource intensive
            //   > each path was to a key, before we recurse:
            //       > clone the grid
            //       > we should reset our "explored" paths
            //       > use that key to unlock its door
            //       > Note: if no paths, then there's no keys left, so store this route's number of steps, and DON'T recurse
            void ProcessChild((char key, int numberOfSteps, Vector location) keyFound)
            {
                var childGrid = grid.CloneWithRefToGrid();
                childGrid.PickUpKeyAndUnlockDoor(keyFound.location);

                if (childGrid.NumberOfKeysRemaining == 0)
                {
                    var prevMinSteps = part1Result.MinNumberOfStepsToCollectAllKeys;
                    lock (mutex)
                    {
                        part1Result.MinNumberOfStepsToCollectAllKeys = Math.Min(part1Result.MinNumberOfStepsToCollectAllKeys, keyFound.numberOfSteps);
                    }
                    if (prevMinSteps != part1Result.MinNumberOfStepsToCollectAllKeys)
                    {
                        Logger.Information("New `Number Of Steps To Collect All Keys` found: " + part1Result.MinNumberOfStepsToCollectAllKeys);
                    }
                }
                else if (keyFound.numberOfSteps < part1Result.MinNumberOfStepsToCollectAllKeys)
                {
                    Explore(childGrid, keyFound.location, keyFound.numberOfSteps, numberOfKeysToFind, false, part1Result, cache);
                }
            }

            if (isFirstLevel)
            {
                Parallel.ForEach(
                    keysFound,
                    new ParallelOptions { MaxDegreeOfParallelism = 4 },
                    ProcessChild);
            }
            else
            {
                foreach (var keyFound in keysFound)
                {
                    ProcessChild(keyFound);
                }
            }
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
