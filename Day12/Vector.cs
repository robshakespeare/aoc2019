namespace Day12
{
    public struct Vector
    {
        public int X { get; }

        public int Y { get; }

        public int Z { get; }

        public Vector(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public override string ToString() => $"<x={X}, y={Y}, z={Z}>";
    }
}
