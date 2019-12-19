using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day18
{
    public static class GridParser
    {
        public static (Grid grid, IReadOnlyList<Vector> startingPositions) Parse(string input)
        {
            var grid = new Dictionary<Vector, char>();
            var doors = new Dictionary<char, Vector>();
            var keys = new Dictionary<char, Vector>();
            List<Vector> positions = new List<Vector>();

            foreach (var tile in input
                .ReadAllLines()
                .SelectMany((line, y) => line.ToCharArray().Select<char, (Vector position, char piece)>((c, x) => (new Vector(x, y), c))))
            {
                if (tile.piece == '@') // @ means our position
                {
                    positions.Add(tile.position);
                    grid.Add(tile.position, '.'); // We can move here
                }
                else
                {
                    grid.Add(tile.position, tile.piece);

                    if (Grid.IsKey(tile.piece))
                    {
                        keys.Add(tile.piece, tile.position);
                    }

                    if (Grid.IsDoor(tile.piece))
                    {
                        doors.Add(tile.piece, tile.position);
                    }
                }
            }

            if (positions.Count == 0)
            {
                throw new InvalidOperationException("Our location(s) not found!");
            }

            return (
                new Grid(
                    grid,
                    new Dictionary<char, Vector>(doors.OrderBy(x => x.Key)),
                    new Dictionary<char, Vector>(keys.OrderBy(x => x.Key))),
                positions);
        }
    }
}
