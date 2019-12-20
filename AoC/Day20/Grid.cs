using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day20
{
    public class Grid
    {
        private readonly Dictionary<Vector, char> tiles;
        private readonly Dictionary<Vector, Warp> portals; // KEY is the warp's portal tiles, VALUE is the warp object itself

        public Vector StartTile { get; }
        public Vector EndTile { get; }

        public Grid(Dictionary<Vector, char> tiles, Dictionary<Vector, Warp> portals, Vector startTile, Vector endTile)
        {
            this.tiles = tiles;
            this.portals = portals;
            StartTile = startTile;
            EndTile = endTile;
        }

        public bool IsAvailableLocation(Vector position) => tiles.TryGetValue(position, out var c) && c == '.';

        public bool IsEndTile(Vector position) => position.Equals(EndTile);

        public Vector PerformPart1WarpIfApplicable(Vector position) => portals.TryGetValue(position, out var warp) ? warp.PerformWarp(position) : position;

        public static Grid Create(string input)
        {
            var tiles = GridParser.Parse(input);

            bool IsLetter(char c) => c >= 'A' && c <= 'Z';
            bool IsAvailableSpace(Vector position) => tiles.TryGetValue(position, out var c) && c == '.';

            // Find individual warp locations
            // Note AA and ZZ are special warps, which are actually just starting ending points
            var warpLocations = new[] {new Vector(1, 0), new Vector(0, 1)}
                .SelectMany(searchDirection => tiles
                    .Where(tile => IsLetter(tile.Value))
                    .Select(tile => new
                    {
                        tile,
                        nextPosition = tile.Key + searchDirection
                    })
                    .Where(tile => tiles.TryGetValue(tile.nextPosition, out var nextPosChar) && IsLetter(nextPosChar))
                    .Select(tile => new
                    {
                        label = $"{tile.tile.Value}{tiles[tile.nextPosition]}",
                        warpTiles = new[] { tile.tile.Key, tile.nextPosition },
                        entryExitPointCandidate1 = tile.nextPosition + searchDirection,
                        entryExitPointCandidate2 = tile.tile.Key - searchDirection,
                        offGridCandidate1 = tile.nextPosition + searchDirection + searchDirection,
                        offGridCandidate2 = tile.tile.Key - searchDirection - searchDirection
                    })
                    .Select(tile => new
                    {
                        tile.label,
                        tile.warpTiles,
                        isOuter = !tiles.ContainsKey(tile.offGridCandidate1) || !tiles.ContainsKey(tile.offGridCandidate2),
                        entryExitPoint =
                            IsAvailableSpace(tile.entryExitPointCandidate1)
                                ? tile.entryExitPointCandidate1
                                : IsAvailableSpace(tile.entryExitPointCandidate2)
                                    ? tile.entryExitPointCandidate2
                                    : throw new InvalidOperationException($"Failed to find entry/exit point for warp {tile.label}")
                    }))
                .ToReadOnlyArray();

            // Get the special start and end tiles
            var startTile = warpLocations.Single(x => x.label == "AA").entryExitPoint;
            var endTile = warpLocations.Single(x => x.label == "ZZ").entryExitPoint;

            // Pair the rest of the warp locations in to Warp objects
            var warpList = warpLocations
                .Where(x => x.label != "AA" && x.label != "ZZ")
                .GroupBy(x => x.label)
                .Select(x =>
                {
                    if (x.Count() != 2)
                    {
                        throw new InvalidOperationException($"Invalid group count of {x.Count()} for {x.Key}");
                    }

                    var outerIndex = x.ElementAt(0).isOuter
                        ? 0
                        : x.ElementAt(1).isOuter
                            ? 1
                            : throw new InvalidOperationException($"Outer not found for {x.Key}");

                    var innerIndex = !x.ElementAt(0).isOuter
                        ? 0
                        : !x.ElementAt(1).isOuter
                            ? 1
                            : throw new InvalidOperationException($"Inner not found for {x.Key}");

                    return new Warp(
                        x.Key,
                        x.ElementAt(outerIndex).entryExitPoint,
                        x.ElementAt(outerIndex).warpTiles,
                        x.ElementAt(innerIndex).entryExitPoint,
                        x.ElementAt(innerIndex).warpTiles);
                });

            // Build dictionary of all the warp entry and exit points
            var portals = new Dictionary<Vector, Warp>();
            foreach (var warp in warpList)
            {
                foreach (var portal in warp.PortalTiles)
                {
                    portals.Add(portal, warp);
                }
            }

            return new Grid(tiles, portals, startTile, endTile);
        }
    }
}
