using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Extensions;
using Common.IntCodes;

namespace AoC.Day17
{
    public class Day17Solver : SolverReadAllText
    {
        private static readonly IntCodeComputer IntCodeComputer = new IntCodeComputer();

        private static readonly Vector NorthNormal = new Vector(0, -1);
        private static readonly Vector SouthNormal = new Vector(0, 1);
        private static readonly Vector EastNormal = new Vector(1, 0);
        private static readonly Vector WestNormal = new Vector(-1, 0);

        /// <summary>
        /// Solve part 1.
        /// </summary>
        public override long? SolvePart1(string inputProgram)
        {
            var (lines, grid) = BuildInitialGrid(inputProgram);

            var intersections = grid
                .Where(p => p.Value == '#')
                .Where(p => new[] {p.Key + NorthNormal, p.Key + SouthNormal, p.Key + EastNormal, p.Key + WestNormal}
                                .Select(s => grid.TryGetValue(s, out var sc) && sc == '#')
                                .Count(hasScaffold => hasScaffold) > 2)
                .ToDictionary(i => i.Key);

            foreach (var intersection in intersections)
            {
                lines[intersection.Key.Y][intersection.Key.X] = 'O';
            }

            DisplayLines(lines);

            return intersections.Sum(intersection => intersection.Key.X * intersection.Key.Y);
        }

        private static (List<StringBuilder> lines, Dictionary<Vector, char> grid) BuildInitialGrid(string inputProgram)
        {
            var lines = new List<StringBuilder>();
            var grid = new Dictionary<Vector, char>();
            var lineBuffer = new StringBuilder();

            void HandleOutput(long outputValue)
            {
                if (outputValue == 10)
                {
                    // new line
                    lines.Add(lineBuffer);
                    lineBuffer = new StringBuilder();
                }
                else
                {
                    var outputChar = (char) outputValue;
                    if (outputChar != '.')
                    {
                        grid.Add(new Vector(lineBuffer.Length, lines.Count), outputChar);
                    }
                    lineBuffer.Append(outputChar);
                }
            }

            IntCodeComputer.ParseAndEvaluateWithSignalling(inputProgram, () => 0, HandleOutput);
            return (lines, grid);
        }

        private static void DisplayLines(IEnumerable<StringBuilder> lines)
        {
            foreach (var line in lines)
            {
                Console.WriteLine(line.ToString());
            }
        }

        private static IntCodeState SeedProgram(string inputProgram, int addressZeroOverrideValue)
        {
            var intCodeState = IntCodeComputer.Parse(inputProgram);
            intCodeState[0] = addressZeroOverrideValue;
            return intCodeState;
        }

        /// <summary>
        /// Solve part 2.
        /// </summary>
        public override long? SolvePart2(string inputProgram)
        {
            var (_, grid) = BuildInitialGrid(inputProgram);

            var heading = NorthNormal;
            var position = grid.Single(x => x.Value == '^').Key;

            var commands = TraceSegments(position, heading, grid).ToReadOnlyArray();

            var commandString = string.Join(",", commands.Select(seg => $"{seg.turn}{seg.move}"));

            Console.WriteLine("Command string: " + commandString);

            // find pattern within the list of commands
            var patternTokens = PatternFinder.FindFirstMatchingPattern(commandString);
            if (patternTokens == null)
            {
                throw new InvalidOperationException("Unable to find pattern!");
            }

            var functionId = new Queue<string>(new[] {"A", "B", "C"});
            var patternTokensWithId = patternTokens
                .Select(patternToken => new
                {
                    patternToken,
                    functionId = functionId.Dequeue(),
                    transcodedPatternToken = TranscodePatternToken(patternToken)
                })
                .ToArray();

            // convert main command to the corresponding letters
            commandString = patternTokensWithId.Aggregate(commandString, (current, token) => current.Replace(token.patternToken, token.functionId));

            Console.WriteLine("Translated pattern command string: " + commandString);
            foreach (var patternTokenWithId in patternTokensWithId)
            {
                Console.WriteLine($"Token {patternTokenWithId.functionId}: {patternTokenWithId.transcodedPatternToken}");
            }

            // Build inputs
            var inputs =
                Encode(commandString)
                    .Concat(Encode(patternTokensWithId[0].transcodedPatternToken))
                    .Concat(Encode(patternTokensWithId[1].transcodedPatternToken))
                    .Concat(Encode(patternTokensWithId[2].transcodedPatternToken))
                    .Concat(Encode("n"))
                    .ToArray();

            // set addressZero to 2, provide the inputs, and go until we have an output, or we finish??
            var intCodeState = IntCodeComputer.Evaluate(SeedProgram(inputProgram, 2), inputs);

            Console.WriteLine("Outputs: ");
            Console.Write(Encoding.ASCII.GetString(intCodeState.Outputs.Where(x => x < 128).Select(x => (byte)x).ToArray()));

            return intCodeState.LastOutputValue;
        }

        private static string TranscodePatternToken(string patternToken) => patternToken.Replace("R", "R,").Replace("L", "L,");

        public long[] Encode(string s) => s.ToCharArray().Select(c => (long)c).Append(10).ToArray();

        private static readonly Dictionary<Vector, (Vector direction, char turn)[]> NextPossibleDirections = new Dictionary<Vector, (Vector nextDirection, char turn)[]>
        {
            {NorthNormal, new[] {(EastNormal, 'R'), (WestNormal, 'L')}},
            {SouthNormal, new[] {(EastNormal, 'L'), (WestNormal, 'R')}},
            {EastNormal, new[] {(NorthNormal, 'L'), (SouthNormal, 'R')}},
            {WestNormal, new[] {(NorthNormal, 'R'), (SouthNormal, 'L')}}
        };

        private static IEnumerable<(char turn, int move)> TraceSegments(Vector position, Vector direction, IReadOnlyDictionary<Vector, char> grid)
        {
            while (true)
            {
                // Find next piece of floor
                var nextMove = NextPossibleDirections[direction]
                    .Select(next => new { next.direction, next.turn, nextPos = position + next.direction })
                    .SingleOrDefault(next => grid.TryGetValue(next.nextPos, out var nextPiece) && nextPiece == '#');

                // If no next piece, then we're done tracing
                if (nextMove == null)
                {
                    yield break;
                }

                // Turn to find next piece of floor
                direction = nextMove.direction;

                // Move across floor until reach end
                var forwardCount = 0;
                while (grid.TryGetValue(position + direction, out var nextPieceInLine) && nextPieceInLine == '#')
                {
                    position += direction;
                    forwardCount++;
                }

                yield return (nextMove.turn, forwardCount);
            }
        }
    }
}
