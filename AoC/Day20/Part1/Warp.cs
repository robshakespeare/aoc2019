using System;
using System.Linq;

namespace AoC.Day20.Part1
{
    /// <summary>
    /// The positions of a warp are the positions where one can enter/exit, and its corresponding exit point.
    /// Perform a warp is one single step, reaching the exit point tile.
    /// i.e. When on an open tile '.' next to one of these labels, a single step can take you to the other open tile '.' with the same label.
    /// </summary>
    public class Warp
    {
        public string Label { get; }
        public Vector EntryExitPointA { get; }
        public Vector EntryExitPointB { get; }
        public Vector[] WarpTilesA { get; }
        public Vector[] WarpTilesB { get; }
        public Vector[] WarpTiles { get; }

        public Warp(string label, Vector entryExitPointA, Vector[] warpTilesA, Vector entryExitPointB, Vector[] warpTilesB)
        {
            Label = label;
            EntryExitPointA = entryExitPointA;
            EntryExitPointB = entryExitPointB;
            WarpTilesA = warpTilesA;
            WarpTilesB = warpTilesB;
            WarpTiles = warpTilesA.Concat(warpTilesB).ToArray();
        }

        public Vector PerformWarp(Vector position) =>
            position switch{
                _ when WarpTilesA.Contains(position) => EntryExitPointB,
                _ when WarpTilesB.Contains(position) => EntryExitPointA,
                _ => throw new InvalidOperationException($"Position {position} is not valid for warp {Label}")
                };

        public override string ToString() => Label;
    }
}
