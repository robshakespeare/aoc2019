using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day20.Part1
{
    public static class GridParser
    {
        public static Dictionary<Vector, char> Parse(string input)
        {
            var grid = new Dictionary<Vector, char>();

            foreach (var (position, tile) in input
                .ReadAllLines()
                .SelectMany((line, y) => line.ToCharArray().Select<char, (Vector position, char tile)>((c, x) => (new Vector(x, y), c))))
            {
                grid.Add(position, tile);
            }

            return grid;
        }
    }
}
