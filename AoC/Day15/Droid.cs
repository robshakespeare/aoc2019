using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Common;
using Common.IntCodes;

namespace AoC.Day15
{
    public class Droid
    {
        private static readonly IntCodeComputer IntCodeComputer = new IntCodeComputer();

        private readonly IntCodeState intCodeState;
        private readonly Dictionary<Vector, int> gridSteps = new Dictionary<Vector, int>(); // KEY is grid location, VALUE is the number of steps from origins
        private readonly Stack<Vector> gridTrail = new Stack<Vector>(); // So we can track backwards, first item out is always our CURRENT position
        private readonly Dictionary<Vector, Queue<MovementCommand>> gridAvailableCommands = new Dictionary<Vector, Queue<MovementCommand>>();
        private readonly HashSet<Vector> gridLocationsWithAvailableCommands = new HashSet<Vector>();
        private readonly Dictionary<Vector, GridState> gridStates = new Dictionary<Vector, GridState>();

        private Vector droidPosition;
        private Vector? nextAttemptedDroidPosition;
        private Vector? oxygenSystemPosition;

        public Droid(IntCodeState intCodeState)
        {
            this.intCodeState = intCodeState;

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
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();

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
            if (oxygenSystemPosition == null)
            {
                throw new InvalidOperationException("Oxygen System not found!");
            }

            var oxygenSystem = new OxygenSystem(oxygenSystemPosition.Value, Offset, gridStates);
            var iterationsToFillWithOxygen = oxygenSystem.FillAndSolve();

            // Display final outputs, and return
            RestoreCursorPosition();
            Console.Write(" Oxygen System found. Number of steps: ");
            var numOfStepsToReachOxygenSystem = gridSteps[oxygenSystemPosition.Value];
            ColorConsole.Write(numOfStepsToReachOxygenSystem, ConsoleColor.Green);

            Console.Write(" || Number of minutes to fill with oxygen: ");
            ColorConsole.WriteLine(iterationsToFillWithOxygen, ConsoleColor.Green);

            return (numOfStepsToReachOxygenSystem, oxygenSystemPosition.Value, iterationsToFillWithOxygen);
        }

        private static void RestoreCursorPosition()
        {
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

        private static void Render(Vector location, char chr)
        {
            var pos = location + Offset;
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(chr);
        }

        private static Queue<MovementCommand> BuildAvailableMovementCommands() => new Queue<MovementCommand>(MovementCommand.PossibleCommands);

        public static Droid Create()
        {
            var inputProgram = new InputLoaderReadAllText(15).LoadInput();
            return new Droid(IntCodeComputer.Parse(inputProgram));
        }
    }

    public enum GridState
    {
        Wall,
        Explored
    }

    public enum ReplyCode
    {
        HitWall = 0,
        Moved = 1,
        MovedAndFoundOxygenSystem = 2
    }

    public class MovementCommand
    {
        static MovementCommand()
        {
            MovementVectorToCommandMap = new Dictionary<Vector, int>
            {
                {North.MovementVector, North.Value},
                {South.MovementVector, South.Value},
                {West.MovementVector, West.Value},
                {East.MovementVector, East.Value}
            };
        }

        public MovementCommand(string name, int value, Vector movementVector)
        {
            Name = name;
            Value = value;
            MovementVector = movementVector;
        }

        public string Name { get; }
        public int Value { get; }
        public Vector MovementVector { get; }

        public override string ToString() => Name;

        public static readonly MovementCommand North = new MovementCommand("North", 1, new Vector(0, -1));
        public static readonly MovementCommand South = new MovementCommand("South", 2, new Vector(0, 1));
        public static readonly MovementCommand West = new MovementCommand("West", 3, new Vector(-1, 0));
        public static readonly MovementCommand East = new MovementCommand("East", 4, new Vector(1, 0));

        public static readonly IReadOnlyCollection<MovementCommand> PossibleCommands =
            new ReadOnlyCollection<MovementCommand>(new[] {North, East, South, West});

        private static readonly Dictionary<Vector, int> MovementVectorToCommandMap;

        public static int MapMovementToCommand(Vector movement)
        {
            if (MovementVectorToCommandMap.TryGetValue(movement, out var command))
            {
                return command;
            }
            throw new InvalidOperationException("Unable to map back to command, invalid movement: " + movement);
        }
    }
}
