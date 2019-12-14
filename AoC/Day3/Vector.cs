using System;
using System.Collections.Generic;

namespace AoC.Day3
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

        public static readonly Vector CentralPort = new Vector(0, 0);

        public int Length => ManhattanDistance(this, CentralPort);

        public Vector Normal => this / Length;

        public static int ManhattanDistance(Vector a, Vector b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public void VisitGrid(Dictionary<Vector, List<(Wire wire, int numberOfSteps)>> grid, Wire wire, int numberOfSteps)
        {
            if (grid.TryGetValue(this, out var list))
            {
                list.Add((wire, numberOfSteps));
            }
            else
            {
                grid.Add(this, new List<(Wire wire, int numberOfSteps)> { (wire, numberOfSteps) });
            }
        }

        public static Vector operator +(Vector a, Vector b)
            => new Vector(a.X + b.X, a.Y + b.Y);

        public static Vector operator /(Vector a, int b)
            => new Vector(a.X / b, a.Y / b);

        public override string ToString() => $"{X},{Y}";
    }
}
