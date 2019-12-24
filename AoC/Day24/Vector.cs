namespace AoC.Day24
{
    public struct Vector
    {
        public int X { get; }

        public int Y { get; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y);

        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y);

        public override string ToString() => $"{X},{Y}";
    }
}
