using System;
using System.Linq;

namespace AoC.Day12
{
    public class Vector
    {
        public const int X = 0;
        public const int Y = 1;
        public const int Z = 2;

        private readonly int[] values;

        public Vector(int x, int y, int z)
        {
            values = new[] {x, y, z};
        }

        public int this[int componentIndex]
        {
            get => values[componentIndex];
            set => values[componentIndex] = value;
        }

        public int SumAbsoluteValuesInVector() => values.Sum(Math.Abs);

        public override string ToString() => $"<x={this[X]}, y={this[Y]}, z={this[Z]}>";
    }
}
