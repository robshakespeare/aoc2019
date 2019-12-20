using System;
using System.Linq;

namespace AoC.Day20
{
    /// <summary>
    /// The positions of a warp are the positions where one can enter/exit, and its corresponding exit point.
    /// Perform a warp is one single step, reaching the exit point tile.
    /// i.e. When on an open tile '.' next to one of these labels, a single step can take you to the other open tile '.' with the same label.
    /// </summary>
    public class Warp
    {
        public string Label { get; }
        public Vector EntryExitPointOuter { get; }
        public Vector EntryExitPointInner { get; }
        public Vector[] PortalTilesOuter { get; }
        public Vector[] PortalTilesInner { get; }
        public Vector[] PortalTiles { get; }

        public Warp(string label, Vector entryExitPointOuter, Vector[] portalTilesOuter, Vector entryExitPointInner, Vector[] portalTilesInner)
        {
            Label = label;
            EntryExitPointOuter = entryExitPointOuter;
            EntryExitPointInner = entryExitPointInner;
            PortalTilesOuter = portalTilesOuter;
            PortalTilesInner = portalTilesInner;
            PortalTiles = portalTilesOuter.Concat(portalTilesInner).ToArray();
        }

        public Vector PerformWarp(Vector position) =>
            position switch
                {
                _ when PortalTilesOuter.Contains(position) => EntryExitPointInner,
                _ when PortalTilesInner.Contains(position) => EntryExitPointOuter,
                _ => throw new InvalidOperationException($"Position {position} is not valid for warp {Label}")
                };

        /// <remarks>
        /// Outer portals take us down a level, i.e. -1
        /// Inner portals take us up a level, i.e. +1
        /// </remarks>
        public (Vector postion, int level, string route) PerformWarpPart2(Vector position, int level, string route) =>
            position switch
                {
                _ when PortalTilesOuter.Contains(position) => (EntryExitPointInner, level - 1, UpdateRoute(route)),
                _ when PortalTilesInner.Contains(position) => (EntryExitPointOuter, level + 1, UpdateRoute(route)),
                _ => throw new InvalidOperationException($"Position {position} is not valid for warp {Label}")
                };

        private string UpdateRoute(string route)
        {
            var lastPortal = GetLastPortalFromRoute(route);
            return $"{route}, {lastPortal} > {Label}";
        }

        public static string GetLastPortalFromRoute(string route) => $"{route[route.Length - 2]}{route[route.Length - 1]}";

        public override string ToString() => Label;
    }
}
