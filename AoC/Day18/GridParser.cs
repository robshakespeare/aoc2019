using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day18
{
    public static class GridParser
    {
        public static (Grid grid, Vector startingPosition) Parse(string input)
        {
            var grid = new Dictionary<Vector, char>();
            var doors = new Dictionary<char, Vector>();
            var keys = new Dictionary<char, Vector>();
            Vector? position = null;

            foreach (var tile in input
                .ReadAllLines()
                .SelectMany((line, y) => line.ToCharArray().Select<char, (Vector position, char piece)>((c, x) => (new Vector(x, y), c))))
            {
                if (tile.piece == '@') // @ means our position
                {
                    position = tile.position;
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

            if (position == null)
            {
                throw new InvalidOperationException("Our location not found!");
            }

            return (
                new Grid(
                    grid,
                    new Dictionary<char, Vector>(doors.OrderBy(x => x.Key)),
                    new Dictionary<char, Vector>(keys.OrderBy(x => x.Key))),
                position.Value);
        }
    }
}
