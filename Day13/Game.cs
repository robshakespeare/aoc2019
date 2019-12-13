using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Day13
{
    public class Game
    {
        public (Vector pos, TileType type)[] InitialTiles { get; }

        public int Left { get; }
        public int Top { get; }
        public int Right { get; }
        public int Bottom { get; }
        public int Width { get; }
        public int Height { get; }
        public Vector TopLeft { get; }

        public Game((Vector pos, TileType type)[] tiles)
        {
            InitialTiles = tiles;

            Left = tiles.Min(t => t.pos.X);
            Top = tiles.Min(t => t.pos.Y);

            Right = tiles.Max(t => t.pos.X) + 1;
            Bottom = tiles.Max(t => t.pos.Y) + 1;

            Width = Right - Left;
            Height = Bottom - Top;
            TopLeft = new Vector(Top, Left);
        }

        public static (Vector pos, TileType type) ParseOutputBatch(IList<long> batch) =>
        (
            new Vector(
                (int) batch[0],
                (int) batch[1]),
            EnumUtil.Parse<TileType>(batch[2])
        );

        public void RenderInitial(bool printSize = false)
        {
            if (printSize)
            {
                Console.WriteLine($"TopLeft: X={Left}, Y={Top}");
                Console.WriteLine($"BottomRight: X={Right}, Y={Bottom}");
            }

            // Index 1 is which line, i.e. Y.
            // Index 2 is which column, i.e. X.
            var buffer = Enumerable
                .Range(0, Height)
                .Select(_ => new string(' ', Width).ToCharArray())
                .ToArray();

            foreach (var (pos, type) in InitialTiles)
            {
                var paintChar = GetPaintChar(type);
                var relLocation = pos - TopLeft;
                buffer[relLocation.Y][relLocation.X] = paintChar;
            }

            var grid = string.Join(
                Environment.NewLine,
                buffer.Select(chars => new string(chars.ToArray())));

            Console.WriteLine(grid);
        }

        private static char GetPaintChar(TileType type)
        {
            var paintChar = type switch
                {
                TileType.Empty => ' ',
                TileType.Wall => '█',
                TileType.Block => '#',
                TileType.Paddle => '¯',
                TileType.Ball => 'o',
                _ => throw new InvalidOperationException("Invalid tileType: " + type)
                };
            return paintChar;
        }

        public void RenderPixel(IList<long> batch)
        {
            var (pos, type) = ParseOutputBatch(batch);

            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(GetPaintChar(type));
        }
    }
}
