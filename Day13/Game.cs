using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Vector BallPrevPosition { get; private set; }
        public Vector BallPosition { get; private set; }
        public Vector BallMovement { get; private set; }

        public Vector PaddlePosition { get; private set; }

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

            BallPosition = new Vector(19, 18); // Hardcoded! This is so our initial movement will be calculated correctly!
        }

        public static (Vector pos, TileType type) ParseOutputBatch(IList<long> batch) =>
        (
            new Vector(
                (int) batch[0],
                (int) batch[1]),
            EnumUtil.Parse<TileType>(batch[2])
        );

        public static char GetPaintChar(TileType type)
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

        public void Update(IList<long> batch)
        {
            var (pos, type) = ParseOutputBatch(batch);

            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(GetPaintChar(type));

            if (type == TileType.Ball)
            {
                BallPrevPosition = BallPosition;
                BallPosition = new Vector(pos.X, pos.Y);
                BallMovement = BallPosition - BallPrevPosition;
            }
            else if (type == TileType.Paddle)
            {
                PaddlePosition = new Vector(pos.X, pos.Y);
            }
        }
    }
}
