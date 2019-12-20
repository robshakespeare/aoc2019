namespace AoC.Day20
{
    public class MovementCommand
    {
        public MovementCommand(string name, Vector movementVector)
        {
            Name = name;
            MovementVector = movementVector;
        }

        public string Name { get; }
        public Vector MovementVector { get; }

        public override string ToString() => Name;

        public static readonly MovementCommand North = new MovementCommand("North", new Vector(0, -1));
        public static readonly MovementCommand South = new MovementCommand("South", new Vector(0, 1));
        public static readonly MovementCommand West = new MovementCommand("West", new Vector(-1, 0));
        public static readonly MovementCommand East = new MovementCommand("East", new Vector(1, 0));
    }
}
