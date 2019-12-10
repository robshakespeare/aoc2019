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

        public double Length => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); // sqrt(vx^2 + vy^2)

        public Vector Abs => new Vector(Math.Abs(X), Math.Abs(Y));

        ////public Vector AbsY => new Vector(X, Math.Abs(Y));

        public static Vector operator +(Vector a, Vector b)
            => new Vector(a.X + b.X, a.Y + b.Y);

        public static Vector operator -(Vector a, Vector b)
            => new Vector(a.X - b.X, a.Y - b.Y);

        public static Vector operator /(Vector a, double b)
            => new Vector(a.X / b, a.Y / b);

        public static Vector operator *(Vector a, double b)
            => new Vector(a.X * b, a.Y * b);

        public override string ToString() => $"{X},{Y}";

        public override bool Equals(object obj)
        {
            return obj is Vector other && Equals(other);
        }

        public bool Equals(Vector other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Vector left, Vector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector left, Vector right)
        {
            return !left.Equals(right);
        }
    }
}
