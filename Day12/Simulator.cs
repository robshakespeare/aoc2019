using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Extensions;

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

        public string BuildPositionAndVelocityText() =>
            string.Join(
                Environment.NewLine,
                moons.Select(moon => $"pos={moon.Position}, vel={moon.Velocity}"));

        public long CalculateTotalEnergyInTheSystem() => moons.Sum(moon => moon.CalculateTotalEnergyForMoon());

        /// <summary>
        /// Applies the necessary calculations to each moon in the simulation, to update a single step.
        /// </summary>
        public void Update()
        {
            foreach (var (moon, velocityChange) in CalculateVelocityChanges())
            {
                moon.Velocity += velocityChange;
                moon.Position += moon.Velocity;
            }
        }

        public (Moon moon, Vector velocityChange)[] CalculateVelocityChanges() =>
            moonPairings
                .Select(moonPairing => (moonPairing.moon, CalculateVelocityChange(moonPairing.moon, moonPairing.otherMoons)))
                .ToArray();
        
        public Vector CalculateVelocityChange(Moon moon, Moon[] otherMoons) =>
            otherMoons.Select(x => x.Position).Aggregate(
                new Vector(),
                (current, otherPosition) => current + CalculateVelocityChange(moon.Position, otherPosition));

        public Vector CalculateVelocityChange(Vector position1, Vector position2) =>
            new Vector(
                CalculateVelocityChange(position1.X, position2.X),
                CalculateVelocityChange(position1.Y, position2.Y),
                CalculateVelocityChange(position1.Z, position2.Z));

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
