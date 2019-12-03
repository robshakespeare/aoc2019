using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Day3
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

        public static int ManhattanDistance(Vector a, Vector b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public void VisitGrid(ConcurrentDictionary<Vector, List<Wire>> grid, Wire wire)
        {
            grid.AddOrUpdate(
                this,
                v => new List<Wire> { wire },
                (v, wires) =>
                {
                    wires.Add(wire);
                    return wires;
                });
        }
    }
}