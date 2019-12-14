using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common;
using Common.Extensions;

namespace AoC.Day12
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

        /// <summary>
        /// Applies the necessary calculations to each moon in the simulation, to update a single step.
        /// </summary>
        public void Update()
        {
            foreach (var componentIndex in new[] {Vector.X, Vector.Y, Vector.Z})
            {
                UpdateComponentForAllMoons(componentIndex);
            }
        }

        public long FindFirstRepeatingStateStepNumber()
        {
            // Find the number of steps which it takes each component to loop
            // And then get the lowest common multiple of those values, which gives us the number of loops it would have
            // to loop by until the entire state matched

            var xLoop = GetNumberOfStepsToLoop(Vector.X);
            var yLoop = GetNumberOfStepsToLoop(Vector.Y);
            var zLoop = GetNumberOfStepsToLoop(Vector.Z);

            return MathUtils.LeastCommonMultiple(xLoop, yLoop, zLoop);
        }

        private long GetNumberOfStepsToLoop(in int componentIndex)
        {
            long stepNumber = 0;
            long? numberOfStepsUntilLoop = null;

            while (numberOfStepsUntilLoop == null)
            {
                stepNumber++;

                // Update all the moon's velocities and positions for this component
                UpdateComponentForAllMoons(componentIndex);

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

        public string BuildPositionAndVelocityText() => string.Join(Environment.NewLine, (IEnumerable<Moon>) moons);

        public long CalculateTotalEnergyInTheSystem() => moons.Sum(moon => moon.CalculateTotalEnergyForMoon());

        /// <summary>
        /// Updates all the moon's velocities and positions for this component, by a single step.
        /// </summary>
        private void UpdateComponentForAllMoons(int componentIndex)
        {
            foreach (var (moonPairing, velocityChange) in CalculateVelocityChanges(componentIndex))
            {
                var moon = moonPairing.moon;
                moon.Velocity[componentIndex] += velocityChange;
                moon.Position[componentIndex] += moon.Velocity[componentIndex];
            }
        }

        private ((Moon moon, Moon[] otherMoons) moonPairing, int)[] CalculateVelocityChanges(int componentIndex) =>
            moonPairings
                .Select(moonPairing => (moonPairing, CalculateVelocityChange(moonPairing.moon, moonPairing.otherMoons, componentIndex)))
                .ToArray();
        
        private static int CalculateVelocityChange(Moon moon, Moon[] otherMoons, int componentIndex) =>
            otherMoons.Select(x => x.Position).Aggregate(
                0,
                (current, otherPosition) => current + CalculateVelocityChange(moon.Position[componentIndex], otherPosition[componentIndex]));

        private static int CalculateVelocityChange(int position1, int position2)
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

        private static (Moon moon, Moon[] otherMoons)[] GetMoonPairings(Moon[] moons) =>
            moons.Select((moon, moonIndex) => (
                    moon,
                    moons.Where(m => m != moon).ToArray()))
                .ToArray();

        private static Moon[] ParseInputLines(string allInputLines) =>
            allInputLines
                .NormalizeLineEndings()
                .ReadAllLines()
                .Select(ParseInputLine)
                .ToArray();

        private static Moon ParseInputLine(string line)
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
