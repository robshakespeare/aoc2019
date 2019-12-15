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
        private readonly Dictionary<Vector, GridState> gridStates = new Dictionary<Vector, GridState>();
        private readonly Dictionary<Vector, int> gridSteps = new Dictionary<Vector, int>(); // KEY is grid location, VALUE is the number of steps from origins
        private readonly Stack<Vector> gridTrail = new Stack<Vector>(); // So we can track backwards, first item out is always our CURRENT position

        private Vector droidPosition;
        private Vector? nextAttemptedDroidPosition;
        private Vector? oxygenSystemPosition;
        private Queue<MovementCommand> availableCommands = RefreshMovementCommands();

        public Droid(IntCodeState intCodeState)
        {
            this.intCodeState = intCodeState;

            droidPosition = new Vector(0, 0); // Start at 0,0.  South is +ve Y, East is +ve X (North is -ve Y, West is -ve X)
            gridSteps.Add(droidPosition, 0); // We have taken zero steps at our starting location
            gridTrail.Push(droidPosition);
        }

        public void Explore()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();

            var explore = true;

            while (explore && IntCodeComputer.EvaluateNextInstruction(intCodeState, GetNextInput, OnNewOutputValue))
            {
                explore = oxygenSystemPosition == null;
            }

            Console.SetCursorPosition(0, 100);
            Console.CursorVisible = true;

            if (oxygenSystemPosition != null)
            {
                Console.Write("Oxygen System found. Number of steps: ");
                ColorConsole.WriteLine(gridSteps[oxygenSystemPosition.Value].ToString(), ConsoleColor.Green);
            }
        }

        private Vector GetNextAttemptedDroidPosition() => nextAttemptedDroidPosition ?? throw new InvalidOperationException("nextAttemptedDroidPosition not available!");

        private GridState GetGridState(Vector gridLocation) => gridStates.TryGetValue(gridLocation, out var state) ? state : GridState.Unexplored;

        private long GetNextInput()
        {
            // Decide next move:

            // If next move takes us to an unexplored area, then try go there (send input to computer).
            // If next move takes us to a wall, don't go there.
            // If next move takes us to an explored area, don't go there.
            if (availableCommands.Count > 0)
            {
                var nextCommand = availableCommands.Dequeue();
                nextAttemptedDroidPosition = droidPosition + nextCommand.MovementVector;

                var nextAttemptedPositionGridState = GetGridState(GetNextAttemptedDroidPosition());

                return nextAttemptedPositionGridState == GridState.Unexplored
                    ? nextCommand.Value // Next grid state is unexplored area, then try go there (send input to computer)
                    : GetNextInput(); // Next grid state is either a wall or somewhere we've been and tracked back from, so don't go there, and try next input
            }

            // If we run out of possible moves, then track back one, and repeat
            availableCommands = RefreshMovementCommands();

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
                    Render(droidPosition, '.');

                    var newStepNumber = gridSteps[droidPosition] + 1;
                    droidPosition = GetNextAttemptedDroidPosition();
                    nextAttemptedDroidPosition = default;

                    gridStates[droidPosition] = GridState.Explored;
                    gridTrail.Push(droidPosition);

                    // Record our number of steps if we haven't been here yet
                    if (!gridSteps.ContainsKey(droidPosition))
                    {
                        gridSteps.Add(droidPosition, newStepNumber);
                    }

                    // We reached a new location, so refresh our available commands
                    availableCommands = RefreshMovementCommands();

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

        private static Queue<MovementCommand> RefreshMovementCommands() => new Queue<MovementCommand>(MovementCommand.PossibleCommands);

        public static Droid Create()
        {
            var inputProgram = new InputLoaderReadAllText(15).LoadInput();
            return new Droid(IntCodeComputer.Parse(inputProgram));
        }
    }

    public enum GridState
    {
        Unexplored,
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
