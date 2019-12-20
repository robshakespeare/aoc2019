using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

            var (initialGrid, initialPositions) = GridParser.Parse(input);

            var cacheBranches = new ConcurrentDictionary<(string positionId, int numberOfSteps, string keysRemaining), char>();

            // KEY is position in grid combined with remaining keys, VALUE is the next reachable keys found from that location
            var cacheKeysFound =
                new ConcurrentDictionary<(string positionId, string keysRemaining), List<(char key, int numberOfSteps, Vector location, Vector[] locations)>>();

            var part1Result = new Part1Result();

            Explore(
                initialGrid,
                initialPositions,
                0,
                initialGrid.NumberOfKeysRemaining,
                true,
                part1Result,
                cacheBranches,
                cacheKeysFound);

            return part1Result.MinNumberOfStepsToCollectAllKeys;
        }

        private void Explore(Grid grid,
            Vector[] positions,
            int numberOfSteps,
            int numberOfKeysToFind,
            bool isFirstLevel,
            Part1Result part1Result,
            ConcurrentDictionary<(string positionId, int numberOfSteps, string keysRemaining), char> cacheBranches,
            ConcurrentDictionary<(string positionId, string keysRemaining), List<(char key, int numberOfSteps, Vector location, Vector[] locations)>> cacheKeysFound)
        {
            if (numberOfSteps >= part1Result.MinNumberOfStepsToCollectAllKeys)
            {
                return;
            }

            if (lastLogTime == null || (stopwatch.Elapsed - lastLogTime.Value).TotalSeconds > 10)
            {
                var info = new
                {
                    numberOfKeysToFind,
                    cacheKeysFoundCount = cacheKeysFound.Count,
                    stopwatch.Elapsed,
                    part1Result.MinNumberOfStepsToCollectAllKeys
                };
                Logger.Information($"State: {info}");
                lastLogTime = stopwatch.Elapsed;
            }

            var keysRemaining = grid.KeysRemaining;

            var positionId = string.Join("", positions);
            var branchId = (positionId, numberOfSteps, keysRemaining);
            if (cacheBranches.TryAdd(branchId, 'y') == false) // TryAdd returns false if the key already exists
            {
                return;
            }

            List<(char key, int numberOfSteps, Vector location, Vector[] locations)> keysFound;
            var explorerId = (positionId, keysRemaining);
            if (cacheKeysFound.TryGetValue(explorerId, out var keysFoundCached))
            {
                keysFound = keysFoundCached;
            }
            else
            {
                var explorer = new Explorer(grid, positions, part1Result);

                keysFound = explorer.Explore();

                // cache it!
                cacheKeysFound.TryAdd(explorerId, keysFound);
            }

            // Then, for each path, we need to repeat that, which will be exponential, but hopefully not too resource intensive
            //   > each path was to a key, before we recurse:
            //       > clone the grid
            //       > we should reset our "explored" paths
            //       > use that key to unlock its door
            //       > Note: if no paths, then there's no keys left, so store this route's number of steps, and DON'T recurse
            void ProcessChild((char key, int numberOfSteps, Vector location, Vector[] locations) keyFound)
            {
                var totalStepsSoFar = keyFound.numberOfSteps + numberOfSteps;

                var childGrid = grid.CloneWithRefToGrid();
                childGrid.PickUpKeyAndUnlockDoor(keyFound.location);

                if (childGrid.NumberOfKeysRemaining == 0)
                {
                    var prevMinSteps = part1Result.MinNumberOfStepsToCollectAllKeys;
                    lock (mutex)
                    {
                        part1Result.MinNumberOfStepsToCollectAllKeys = Math.Min(part1Result.MinNumberOfStepsToCollectAllKeys, totalStepsSoFar);
                    }
                    if (prevMinSteps != part1Result.MinNumberOfStepsToCollectAllKeys)
                    {
                        Logger.Information("New `Number Of Steps To Collect All Keys` found: " + part1Result.MinNumberOfStepsToCollectAllKeys);
                    }
                }
                else if (totalStepsSoFar < part1Result.MinNumberOfStepsToCollectAllKeys)
                {
                    Explore(childGrid, keyFound.locations, totalStepsSoFar, numberOfKeysToFind, false, part1Result, cacheBranches, cacheKeysFound);
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

        public override long? SolvePart2(string _)
        {
            var input = new InputLoaderReadAllText(18).LoadInputSeparatePart2();

            var (initialGrid, initialPositions) = GridParser.Parse(input);

            var cacheBranches = new ConcurrentDictionary<(string positionId, int numberOfSteps, string keysRemaining), char>();

            // KEY is position in grid combined with remaining keys, VALUE is the next reachable keys found from that location
            var cacheKeysFound =
                new ConcurrentDictionary<(string positionId, string keysRemaining), List<(char key, int numberOfSteps, Vector location, Vector[] locations)>>();

            var part1Result = new Part1Result();

            Explore(
                initialGrid,
                initialPositions,
                0,
                initialGrid.NumberOfKeysRemaining,
                true,
                part1Result,
                cacheBranches,
                cacheKeysFound);

            return part1Result.MinNumberOfStepsToCollectAllKeys;
        }
    }
}
