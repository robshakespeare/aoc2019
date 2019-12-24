using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Extensions;

namespace AoC.Day24
{
    public class Grid
    {
        private readonly Dictionary<Vector, Tile> tiles;

        public Grid(IEnumerable<Tile> tiles)
        {
            Tiles = tiles.ToArray();
            this.tiles = Tiles.ToDictionary(tile => tile.Position);
        }

        protected Tile[] Tiles { get; }

        public virtual void Update()
        {
            foreach (var tile in Tiles)
            {
                tile.UpdateAdjacentBugCount(this);
            }

            foreach (var tile in Tiles)
            {
                tile.UpdateInfestedState();
            }
        }

        protected Tile? TryGetTile(Vector position) => tiles.TryGetValue(position, out var tile) ? tile : null;

        /// <summary>
        /// Counts the number of infested tiles within this single grid.
        /// </summary>
        public int CountInfestedTiles() => Tiles.Count(tile => tile.IsInfested);

        /// <summary>
        /// Returns true if there are any infected tiles within this single grid.
        /// </summary>
        /// <returns></returns>
        public bool IsInfested() => Tiles.Any(tile => tile.IsInfested);

        public long CalculateBiodiversityRating() =>
            Tiles.Select((tile, index) => IsInfested(tile.Position) ? (long)Math.Pow(2, index) : 0)
                .Sum();

        /// <summary>
        /// Returns true if the specified position within this single grid is infected.
        /// </summary>
        public bool IsInfested(Vector position) => tiles.TryGetValue(position, out var tile) && tile.IsInfested;

        // "four adjacent tiles", so assuming North, South, East and West
        public virtual int CountAdjacentBugs(Vector position) =>
            new[]
            {
                position + MovementCommand.North.MovementVector,
                position + MovementCommand.East.MovementVector,
                position + MovementCommand.South.MovementVector,
                position + MovementCommand.West.MovementVector
            }.Count(IsInfested);

        public virtual string Render()
        {
            var buffer = new StringBuilder();
            var prevY = (int?) null;
            foreach (var tile in Tiles)
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

        public static Grid Load(string input) => new Grid(Parse(input));

        public static IEnumerable<Tile> Parse(string input) =>
            input.NormalizeLineEndings().ReadAllLines()
                .SelectMany((line, y) => line.ToCharArray().Select((c, x) => new Tile(new Vector(x, y), c == '#')));
    }
}
