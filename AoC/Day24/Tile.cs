using System;

namespace AoC.Day24
{
    public class Tile
    {
        private int? adjacentBugCount;

        public Tile(Vector position, bool isInfested)
        {
            Position = position;
            IsInfested = isInfested;
        }

        public Vector Position { get; }
        public bool IsInfested { get; private set; }

        public void UpdateAdjacentBugCount(Grid grid)
        {
            if (adjacentBugCount != null)
            {
                throw new InvalidOperationException($"Call {nameof(UpdateInfestedState)} next.");
            }

            adjacentBugCount = grid.CountAdjacentBugs(Position);
        }

        public void UpdateInfestedState()
        {
            if (adjacentBugCount == null)
            {
                throw new InvalidOperationException($"Call {nameof(UpdateAdjacentBugCount)} first.");
            }

            if (IsInfested)
            {
                // An infested space dies (becoming an empty space) unless there is exactly ONE infested adjacent to it.
                IsInfested = adjacentBugCount == 1;
            }
            else
            {
                // An empty space becomes infested if exactly ONE or TWO bugs are adjacent to it.
                IsInfested = adjacentBugCount == 1 || adjacentBugCount == 2;
            }

            adjacentBugCount = null;
        }
    }
}
