using System;

namespace Day10
{
    public struct Vector
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector Normal => this / Length;

        public double Length => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); // sqrt(vx^2 + vy^2)

        public Vector Abs => new Vector(Math.Abs(X), Math.Abs(Y));

        public static Vector operator +(Vector a, Vector b)
            => new Vector(a.X + b.X, a.Y + b.Y);

        public static Vector operator /(Vector a, double b)
            => new Vector(a.X / b, a.Y / b);

        public static Vector operator *(Vector a, double b)
            => new Vector(a.X * b, a.Y * b);

        public override string ToString() => $"{X},{Y}";
    }
}
