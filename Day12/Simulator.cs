using System;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Extensions;

namespace Day12
{
    public class Simulator
    {
        private static readonly Regex ParseInputLineRegex = new Regex("<x=(?<x>[^,]+), y=(?<y>[^,]+), z=(?<z>[^,]+)>", RegexOptions.Compiled);

        private readonly Moon[] moons;
        private readonly (Moon a, Moon b)[] moonPairs;

        public Simulator(string allInputLines)
        {
            moons = ParseInputLines(allInputLines);
            moonPairs = GetMoonPairs(moons);
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

        /// <summary>
        /// Applies the necessary calculations to each moon in the simulation, to update a single step.
        /// </summary>
        public void Update()
        {
            foreach (var moonPair in moonPairs)
            {
                Update(moonPair);
            }
        }

        public void Update((Moon a, Moon b) moonPair)
        {
            var (moonA, moonB) = moonPair;

            var velocityChangeA = CalculateVelocityChange(moonA.Position, moonB.Position);
            var velocityChangeB = CalculateVelocityChange(moonB.Position, moonA.Position);

            moonA.Velocity += velocityChangeA;
            moonA.Position += moonA.Velocity;

            moonB.Velocity += velocityChangeB;
            moonB.Position += moonB.Velocity;
        }

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

        public static (Moon a, Moon b)[] GetMoonPairs(Moon[] moons)
        {
            if (moons.Length != 4)
            {
                throw new NotSupportedException("Moon simulator doesn't support anything but 4 moons yet!");
            }

            // Assuming every neighbor pair.
            ////return new[]
            ////{
            ////    (moons[0], moons[1]),
            ////    (moons[1], moons[2]),
            ////    (moons[2], moons[3]),
            ////    (moons[3], moons[0])
            ////};

            // Assuming that it means every possible pair, not just every neighbor pair, or every contiguous pair.
            return new[]
            {
                (moons[0], moons[1]),
                (moons[0], moons[2]),
                (moons[0], moons[3]),

                (moons[1], moons[2]),
                (moons[1], moons[3]),

                (moons[2], moons[3])
            };
        }

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
