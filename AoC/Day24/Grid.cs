using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Extensions;

namespace AoC.Day24
{
    public class Grid
    {
        private readonly Tile[] tilesArray;
        private readonly Dictionary<Vector, Tile> tiles;

        public Grid(IEnumerable<Tile> tiles)
        {
            tilesArray = tiles.ToArray();
            this.tiles = tilesArray.ToDictionary(tile => tile.Position);
        }

        public void Update()
        {
            foreach (var tile in tilesArray)
            {
                tile.UpdateAdjacentBugCount(this);
            }

            foreach (var tile in tilesArray)
            {
                tile.UpdateInfestedState();
            }
        }

        public long CalculateBiodiversityRating() =>
            tilesArray.Select((tile, index) => IsInfested(tile.Position) ? (long)Math.Pow(2, index) : 0)
                .Sum();

        public bool IsInfested(Vector position) => tiles.TryGetValue(position, out var tile) && tile.IsInfested;

        // "four adjacent tiles", so assuming North, South, East and West
        public int CountAdjacentBugs(Vector position) =>
            new[]
            {
                position + MovementCommand.North.MovementVector,
                position + MovementCommand.East.MovementVector,
                position + MovementCommand.South.MovementVector,
                position + MovementCommand.West.MovementVector
            }.Count(IsInfested);

        public string Render()
        {
            var buffer = new StringBuilder();
            var prevY = (int?) null;
            foreach (var tile in tilesArray)
            {
                if (prevY != null && prevY != tile.Position.Y)
                {
                    buffer.AppendLine();
                }

                buffer.Append(tile.IsInfested ? '#' : '.');
                prevY = tile.Position.Y;
            }

            return buffer.ToString();
        }

        public static Grid Load(string input) =>
            new Grid(input.NormalizeLineEndings().ReadAllLines()
                .SelectMany((line, y) => line.ToCharArray().Select((c, x) => new Tile(new Vector(x, y), c == '#'))));
    }
}
