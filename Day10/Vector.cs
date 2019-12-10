using System;

namespace Day10
{
    public struct Vector
    {
        public double X { get; }

        public double Y { get; }

        public Vector(double x, double y)
        {
            X = Math.Round(x, 14);
            Y = Math.Round(y, 14);
        }

        public Vector Normal => this / Length;

        public static Vector UpNormal = new Vector(0, -1d); // Note -1 is up, because positive Y goes down in these map coordinate system

        public static double AngleBetween(Vector vector1, Vector vector2)
        {
            var sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            var cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            var angleBetween = Math.Atan2(sin, cos) * (180 / Math.PI);
            return angleBetween < 0 ? 360 + angleBetween : angleBetween;
        }

        public double Length => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); // sqrt(vx^2 + vy^2)

        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y);

        public static Vector operator /(Vector a, double b) => new Vector(a.X / b, a.Y / b);

        public override string ToString() => $"{X},{Y}";
    }
}
