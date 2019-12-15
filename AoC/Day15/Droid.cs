using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.IntCodes;

namespace AoC.Day15
{
    public class Droid
    {
        private static readonly IntCodeComputer IntCodeComputer = new IntCodeComputer();

        private readonly IntCodeState intCodeState;
        private readonly bool disablePlotting;
        private readonly Dictionary<Vector, int> gridSteps = new Dictionary<Vector, int>(); // KEY is grid location, VALUE is the number of steps from origins
        private readonly Stack<Vector> gridTrail = new Stack<Vector>(); // So we can track backwards, first item out is always our CURRENT position
        private readonly Dictionary<Vector, Queue<MovementCommand>> gridAvailableCommands = new Dictionary<Vector, Queue<MovementCommand>>();
        private readonly HashSet<Vector> gridLocationsWithAvailableCommands = new HashSet<Vector>();
        private readonly Dictionary<Vector, GridState> gridStates = new Dictionary<Vector, GridState>();

        private Vector droidPosition;
        private Vector? nextAttemptedDroidPosition;
        private Vector? oxygenSystemPosition;

        public Droid(IntCodeState intCodeState, bool disablePlotting)
        {
            this.intCodeState = intCodeState;
            this.disablePlotting = disablePlotting;

            droidPosition = new Vector(0, 0); // Start at 0,0.  South is +ve Y, East is +ve X (North is -ve Y, West is -ve X)
            VisitGridLocationForFirstTime(droidPosition, 0); // Note: we have taken zero steps at our starting location
            gridTrail.Push(droidPosition);
        }

        private void VisitGridLocationForFirstTime(Vector location, int newStepNumber)
        {
            gridSteps.Add(location, newStepNumber);
            gridAvailableCommands.Add(location, BuildAvailableMovementCommands());
            gridLocationsWithAvailableCommands.Add(location);
        }

        public (int numOfStepsToReachOxygenSystem, Vector oxygenSystemPosition, long iterationsToFillWithOxygen) ExploreAndSolve()
        {
            if (!disablePlotting)
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.CursorVisible = false;
                Console.Clear();
            }

            try
            {
                Explore();
                return Solve();
            }
            finally
            {
                RestoreCursorPosition();
            }
        }

        private void Explore()
        {
            while (gridLocationsWithAvailableCommands.Count > 0 &&
                   IntCodeComputer.EvaluateNextInstruction(intCodeState, GetNextInput, OnNewOutputValue))
            {
            }
        }

        private (int numOfStepsToReachOxygenSystem, Vector oxygenSystemPosition, long iterationsToFillWithOxygen) Solve()
        {
            var oxygenSystemPos = oxygenSystemPosition ?? throw new InvalidOperationException("Oxygen System not found!");
            var oxygenSystem = new OxygenSystem(oxygenSystemPos, Offset, gridStates, disablePlotting);
            var iterationsToFillWithOxygen = oxygenSystem.FillAndSolve();

            // Display final outputs, and return
            RestoreCursorPosition();
            Console.Write(" Oxygen System found. Number of steps: ");
            var numOfStepsToReachOxygenSystem = gridSteps[oxygenSystemPos];
            ColorConsole.Write(numOfStepsToReachOxygenSystem, ConsoleColor.Green);

            Console.Write(" || Number of minutes to fill with oxygen: ");
            ColorConsole.WriteLine(iterationsToFillWithOxygen, ConsoleColor.Green);

            return (numOfStepsToReachOxygenSystem, oxygenSystemPos, iterationsToFillWithOxygen);
        }

        private void RestoreCursorPosition()
        {
            if (disablePlotting)
            {
                return;
            }

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, 101);
        }

        private Vector GetNextAttemptedDroidPosition() => nextAttemptedDroidPosition ?? throw new InvalidOperationException("nextAttemptedDroidPosition not available!");

        private long GetNextInput()
        {
            // Decide next move:

            // If next move takes us to an unexplored area, then try go there (send input to computer).
            // If next move takes us to a wall, don't go there.
            // If next move takes us to an explored area, don't go there.

            var availableCommands = gridAvailableCommands[droidPosition];

            if (availableCommands.Count > 0)
            {
                var nextCommand = availableCommands.Dequeue();
                nextAttemptedDroidPosition = droidPosition + nextCommand.MovementVector;
                return nextCommand.Value;
            }

            // If we run out of possible moves, then track back one, and repeat
            var currentPosition = gridTrail.Pop();
            nextAttemptedDroidPosition = gridTrail.Pop();
            var movementBack = GetNextAttemptedDroidPosition() - currentPosition;
            return MovementCommand.MapMovementToCommand(movementBack);
        }

        private void OnNewOutputValue(long outputValue)
        {
            var replyCode = EnumUtil.Parse<ReplyCode>(outputValue);

            switch (replyCode)
            {
                case ReplyCode.Moved:
                case ReplyCode.MovedAndFoundOxygenSystem:
                {
                    Render(droidPosition, droidPosition.Equals(new Vector(0, 0)) ? 'S' : '.');

                    if (oxygenSystemPosition != null)
                    {
                        Render(oxygenSystemPosition.Value, 'X');
                    }

                    if (gridAvailableCommands[droidPosition].Count == 0)
                    {
                        gridLocationsWithAvailableCommands.Remove(droidPosition);
                    }

                    var newStepNumber = gridSteps[droidPosition] + 1;
                    droidPosition = GetNextAttemptedDroidPosition();
                    nextAttemptedDroidPosition = default;

                    gridStates[droidPosition] = GridState.Explored;
                    gridTrail.Push(droidPosition);

                    // Record our number of steps if we haven't been here yet
                    if (!gridSteps.ContainsKey(droidPosition))
                    {
                        VisitGridLocationForFirstTime(droidPosition, newStepNumber);
                    }

                    // If we found the oxygen system, woo hoo, job done
                    if (replyCode == ReplyCode.MovedAndFoundOxygenSystem)
                    {
                        oxygenSystemPosition = droidPosition;
                    }

                    Render(droidPosition, 'D');
                    break;
                }
                default:
                {
                    gridStates[GetNextAttemptedDroidPosition()] = GridState.Wall;
                    Render(GetNextAttemptedDroidPosition(), '█');
                    nextAttemptedDroidPosition = default;
                    break;
                }
            }
        }

        private static readonly Vector Offset = new Vector(80, 80);

        private void Render(Vector location, char chr)
        {
            if (disablePlotting)
            {
                return;
            }

            var pos = location + Offset;
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(chr);
        }

        private static Queue<MovementCommand> BuildAvailableMovementCommands() => new Queue<MovementCommand>(MovementCommand.PossibleCommands);

        public static Droid Create(bool disablePlotting = false)
        {
            var inputProgram = new InputLoaderReadAllText(15).LoadInput();
            return new Droid(IntCodeComputer.Parse(inputProgram), disablePlotting);
        }
    }
}
