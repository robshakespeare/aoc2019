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

        public long FindFirstRepeatingStateStepNumber()
        {
            var states = new Dictionary<int, Dictionary<int, Dictionary<int, HashSet<int>>>>();

            long? firstRepeatingStateStepNumber = null;
            long stepNumber = 0;

            while (firstRepeatingStateStepNumber == null)
            {
                Update();

                var moon1State = moons[0].GetState();
                var moon2State = moons[1].GetState();
                var moon3State = moons[2].GetState();
                var moon4State = moons[3].GetState();


                var moon1StateMatch = false;

                if (!states.TryGetValue(moon1State, out var moon1StateDict))
                {
                    states[moon1State] = moon1StateDict = new Dictionary<int, Dictionary<int, HashSet<int>>>();
                }
                else
                {
                    moon1StateMatch = true;
                }


                var moon2StateMatch = false;

                if (!moon1StateDict.TryGetValue(moon2State, out var moon2StateDict))
                {
                    moon1StateDict[moon2State] = moon2StateDict = new Dictionary<int, HashSet<int>>();
                }
                else
                {
                    moon2StateMatch = true;
                }


                var moon3StateMatch = false;

                if (!moon2StateDict.TryGetValue(moon3State, out var moon3StateDict))
                {
                    moon2StateDict[moon3State] = moon3StateDict = new HashSet<int>();
                }
                else
                {
                    moon3StateMatch = true;
                }


                var moon4StateMatch = false;

                if (!moon3StateDict.Contains(moon4State))
                {
                    moon3StateDict.Add(moon4State);
                }
                else
                {
                    moon4StateMatch = true;
                }

                if (moon1StateMatch &&
                    moon2StateMatch &&
                    moon3StateMatch &&
                    moon4StateMatch)
                {
                    firstRepeatingStateStepNumber = stepNumber;
                }

                stepNumber++;
            }

            return firstRepeatingStateStepNumber.Value;
        }

        ////public long FindFirstRepeatingStateStepNumber()
        ////{
        ////    var shifter = (ulong)int.MaxValue + int.MaxValue;

        ////    var hashSet = new HashSet<ulong>();

        ////    long? firstRepeatingStateStepNumber = null;
        ////    long stepNumber = 0;

        ////    while (firstRepeatingStateStepNumber == null)
        ////    {
        ////        Update();
        ////        stepNumber++;
        ////        var state = moons[0].GetState() |
        ////                    (moons[1].GetState() + shifter) |
        ////                    (moons[2].GetState() + shifter * 2) |
        ////                    (moons[3].GetState() + shifter * 3);

        ////        var alreadyPresent = hashSet.Add(state) == false;

        ////        if (alreadyPresent)
        ////        {
        ////            firstRepeatingStateStepNumber = stepNumber;
        ////        }
        ////    }

        ////    return firstRepeatingStateStepNumber.Value;
        ////}

        public string BuildPositionAndVelocityText() => string.Join(Environment.NewLine, (IEnumerable<Moon>) moons);

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
