using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Extensions;
using static Day12.Vector;

namespace Day12
{
    public class Simulator
    {
        private static readonly Regex ParseInputLineRegex = new Regex("<x=(?<x>[^,]+), y=(?<y>[^,]+), z=(?<z>[^,]+)>", RegexOptions.Compiled);

        private readonly Moon[] moons;
        private readonly (Moon moon, Moon[] otherMoons)[] moonPairings;

        public Simulator(string allInputLines)
        {
            moons = ParseInputLines(allInputLines);
            moonPairings = GetMoonPairings(moons);
        }

        public void RunSimulation(int numberOfSteps)
        {
            for (var stepIndex = 0; stepIndex < numberOfSteps; stepIndex++)
            {
                Update();
            }
        }

        public static Action<string> Logger = Console.WriteLine;

        public long FindFirstRepeatingStateStepNumber()
        {
            var mutex = new object();
            var matches = new SortedList<long, bool[]>(); // KEY = StepNumber, VALUE = which components match for position & velocity
            long? firstRepeatingStateStepNumber = null;

            void AddMatch(long matchStepNumber, int componentIndex)
            {
                lock (mutex)
                {
                    {
                        if (!matches.TryGetValue(matchStepNumber, out var match))
                        {
                            matches[matchStepNumber] = match = new bool[3];
                        }

                        match[componentIndex] = true;
                    }

                    // Check for all matches
                    if (firstRepeatingStateStepNumber == null)
                    {
                        foreach (var match in matches)
                        {
                            if (match.Value[X] && match.Value[Y] && match.Value[Z])
                            {
                                firstRepeatingStateStepNumber = match.Key;
                                return;
                            }
                        }
                    }
                }
            }

            var stopwatch = Stopwatch.StartNew();
            var lastLogTime = stopwatch.Elapsed;

            Parallel.ForEach(
                new[] {X, Y, Z},
                componentIndex =>
                {
                    long stepNumber = 0;
                    while (firstRepeatingStateStepNumber == null)
                    {
                        stepNumber++;
                        var numMoonMatches = 0;

                        foreach (var (moon, velocityChange) in CalculateVelocityChanges(componentIndex))
                        {
                            moon.Velocity[componentIndex] += velocityChange;
                            moon.Position[componentIndex] += moon.Velocity[componentIndex];

                            if (moon.Position[componentIndex] == moon.InitialPosition[componentIndex] &&
                                moon.Velocity[componentIndex] == moon.InitialVelocity[componentIndex])
                            {
                                numMoonMatches++;
                            }
                        }

                        if (numMoonMatches == moons.Length)
                        {
                            AddMatch(stepNumber, componentIndex);
                        }

                        if ((stopwatch.Elapsed - lastLogTime).TotalMilliseconds > 250)
                        {
                            Logger("Finding First Repeating State Step Number. " + new { stepNumber, stopwatch.Elapsed });
                            lastLogTime = stopwatch.Elapsed;
                        }
                    }
                });

            return firstRepeatingStateStepNumber ?? -1;
        }

        public string BuildPositionAndVelocityText() => string.Join(Environment.NewLine, (IEnumerable<Moon>) moons);

        public long CalculateTotalEnergyInTheSystem() => moons.Sum(moon => moon.CalculateTotalEnergyForMoon());

        /// <summary>
        /// Applies the necessary calculations to each moon in the simulation, to update a single step.
        /// </summary>
        public void Update()
        {
            foreach (var componentIndex in new[] {X, Y, Z})
            {
                foreach (var (moon, velocityChange) in CalculateVelocityChanges(componentIndex))
                {
                    moon.Velocity[componentIndex] += velocityChange;
                    moon.Position[componentIndex] += moon.Velocity[componentIndex];
                }
            }
        }

        public (Moon moon, int velocityChange)[] CalculateVelocityChanges(int componentIndex) =>
            moonPairings
                .Select(moonPairing => (moonPairing.moon, CalculateVelocityChange(moonPairing.moon, moonPairing.otherMoons, componentIndex)))
                .ToArray();
        
        public int CalculateVelocityChange(Moon moon, Moon[] otherMoons, int componentIndex) =>
            otherMoons.Select(x => x.Position).Aggregate(
                0,
                (current, otherPosition) => current + CalculateVelocityChange(moon.Position[componentIndex], otherPosition[componentIndex]));

        public int CalculateVelocityChange(int position1, int position2)
        {
            if (position2 > position1)
            {
                return 1;
            }

            if (position2 < position1)
            {
                return -1;
            }

            return 0;
        }

        public static (Moon moon, Moon[] otherMoons)[] GetMoonPairings(Moon[] moons) =>
            moons.Select(moon => (
                    moon,
                    moons.Where(m => m != moon).ToArray()))
                .ToArray();

        public static Moon[] ParseInputLines(string allInputLines) =>
            allInputLines
                .NormalizeLineEndings()
                .ReadAllLines()
                .Select(ParseInputLine)
                .ToArray();

        public static Moon ParseInputLine(string line)
        {
            var match = ParseInputLineRegex.Match(line);

            if (match.Success)
            {
                return new Moon(new Vector(
                    int.Parse(match.Groups["x"].Value),
                    int.Parse(match.Groups["y"].Value),
                    int.Parse(match.Groups["z"].Value)));
            }

            throw new InvalidOperationException("Invalid input line: " + line);
        }
    }
}
