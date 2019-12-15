using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AoC.Day15
{
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
