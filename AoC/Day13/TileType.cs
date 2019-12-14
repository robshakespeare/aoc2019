namespace AoC.Day13
{
    public enum TileType
    {
        /// <summary>
        /// 0 is an empty tile. No game object appears in this tile.
        /// </summary>
        Empty = 0,

        /// <summary>
        /// 1 is a wall tile. Walls are indestructible barriers.
        /// </summary>
        Wall = 1,

        /// <summary>
        /// 2 is a block tile. Blocks can be broken by the ball.
        /// </summary>
        Block = 2,

        /// <summary>
        /// 3 is a horizontal paddle tile. The paddle is indestructible.
        /// </summary>
        Paddle = 3,

        /// <summary>
        /// 4 is a ball tile. The ball moves diagonally and bounces off objects.
        /// </summary>
        Ball = 4
    }
}
