using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            // Find the number of steps which it takes each component to loop
            var xLoop = GetNumberOfStepsToLoop(X);
            var yLoop = GetNumberOfStepsToLoop(Y);
            var zLoop = GetNumberOfStepsToLoop(Z);

            // Use Euclid's algorithm to calculate the greatest common divisor (GCD) of two numbers.
            long GreatestCommonDivisor(long a, long b)
            {
                a = Math.Abs(a);
                b = Math.Abs(b);

                // Pull out remainders.
                for (; ; )
                {
                    long remainder = a % b;
                    if (remainder == 0) return b;
                    a = b;
                    b = remainder;
                }
            }

            // Return the least common multiple (LCM) of two numbers.
            long LeastCommonMultiple(long a, long b) => a * b / GreatestCommonDivisor(a, b);

            // LCM - get the lowest common multiple!
            return LeastCommonMultiple(LeastCommonMultiple(xLoop, yLoop), zLoop);
        }

        private long GetNumberOfStepsToLoop(in int componentIndex)
        {
            long stepNumber = 0;
            long? numberOfStepsUntilLoop = null;

            while (numberOfStepsUntilLoop == null)
            {
                stepNumber++;

                // Update all the moons velocities and positions for this component
                // rs-todo: this is duplicated below, remove the duplicate
                foreach (var (moonPairing, velocityChange) in CalculateVelocityChanges(componentIndex))
                {
                    var moon = moonPairing.moon;
                    moon.Velocity[componentIndex] += velocityChange;
                    moon.Position[componentIndex] += moon.Velocity[componentIndex];
                }

                // Check whether it matches the initial state
                var allMatch = true;
                foreach (var moon in moons)
                {
                    allMatch &= moon.Position[componentIndex] == moon.InitialPosition[componentIndex] &&
                                moon.Velocity[componentIndex] == moon.InitialVelocity[componentIndex];
                }

                if (allMatch)
                {
                    numberOfStepsUntilLoop = stepNumber;
                }
            }

            return numberOfStepsUntilLoop.Value;
        }

        ////public long FindFirstRepeatingStateStepNumber()
        ////{
        ////    var mutex = new object();
        ////    var matches = new SortedList<long, bool[]>(); // KEY = StepNumber, VALUE = which components match for position & velocity
        ////    long? firstRepeatingStateStepNumber = null;

        ////    void AddMatch(long matchStepNumber, int componentIndex)
        ////    {
        ////        lock (mutex)
        ////        {
        ////            {
        ////                if (!matches.TryGetValue(matchStepNumber, out var match))
        ////                {
        ////                    matches[matchStepNumber] = match = new bool[3];
        ////                }

        ////                match[componentIndex] = true;
        ////            }

        ////            // Check for all matches
        ////            if (firstRepeatingStateStepNumber == null)
        ////            {
        ////                foreach (var match in matches)
        ////                {
        ////                    if (match.Value[X] && match.Value[Y] && match.Value[Z])
        ////                    {
        ////                        firstRepeatingStateStepNumber = match.Key;
        ////                        return;
        ////                    }
        ////                }
        ////            }
        ////        }
        ////    }

        ////    var stopwatch = Stopwatch.StartNew();
        ////    var lastLogTime = stopwatch.Elapsed;

        ////    Parallel.ForEach(
        ////        new[] {X, Y, Z},
        ////        componentIndex =>
        ////        {
        ////            long stepNumber = 0;
        ////            while (firstRepeatingStateStepNumber == null)
        ////            {
        ////                stepNumber++;
        ////                var numMoonMatches = 0;

        ////                foreach (var (moon, velocityChange) in CalculateVelocityChanges(componentIndex))
        ////                {
        ////                    moon.Velocity[componentIndex] += velocityChange;
        ////                    moon.Position[componentIndex] += moon.Velocity[componentIndex];

        ////                    if (moon.Position[componentIndex] == moon.InitialPosition[componentIndex] &&
        ////                        moon.Velocity[componentIndex] == moon.InitialVelocity[componentIndex])
        ////                    {
        ////                        numMoonMatches++;
        ////                    }
        ////                }

        ////                if (numMoonMatches == moons.Length)
        ////                {
        ////                    AddMatch(stepNumber, componentIndex);
        ////                }

        ////                if ((stopwatch.Elapsed - lastLogTime).TotalMilliseconds > 250)
        ////                {
        ////                    Logger("Finding First Repeating State Step Number. " + new { stepNumber, stopwatch.Elapsed });
        ////                    lastLogTime = stopwatch.Elapsed;
        ////                }
        ////            }
        ////        });

        ////    return firstRepeatingStateStepNumber ?? -1;
        ////}

        public string BuildPositionAndVelocityText() => string.Join(Environment.NewLine, (IEnumerable<Moon>) moons);

        public long CalculateTotalEnergyInTheSystem() => moons.Sum(moon => moon.CalculateTotalEnergyForMoon());

        /// <summary>
        /// Applies the necessary calculations to each moon in the simulation, to update a single step.
        /// </summary>
        public void Update()
        {
            foreach (var componentIndex in new[] {X, Y, Z})
            {
                foreach (var (moonPairing, velocityChange) in CalculateVelocityChanges(componentIndex))
                {
                    var moon = moonPairing.moon;
                    moon.Velocity[componentIndex] += velocityChange;
                    moon.Position[componentIndex] += moon.Velocity[componentIndex];
                }
            }
        }

        public ((Moon moon, Moon[] otherMoons) moonPairing, int)[] CalculateVelocityChanges(int componentIndex) =>
            moonPairings
                .Select(moonPairing => (moonPairing, CalculateVelocityChange(moonPairing.moon, moonPairing.otherMoons, componentIndex)))
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
            moons.Select((moon, moonIndex) => (
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
