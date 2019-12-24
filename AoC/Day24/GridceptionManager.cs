using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Day24
{
    public class GridceptionManager
    {
        private readonly Tile[] initialTiles;
        private readonly SortedList<int, Gridception> grids;
        private readonly List<Gridception> gridsAddedThisUpdate = new List<Gridception>();

        private GridceptionManager(
            Tile[] initialTiles,
            GridceptionSpec spec)
        {
            this.initialTiles = initialTiles;
            Spec = spec;
            grids = new SortedList<int, Gridception>
            {
                {0, new Gridception(0, this, spec, initialTiles)}
            };
        }

        /// <summary>
        /// Counts the number of infested tiles within all the grids.
        /// </summary>
        public int CountInfestedTiles() => grids.Values.Sum(grid => grid.CountInfestedTiles());

        public void Update()
        {
            // First update the adjacent infected count for all existing levels and any new levels.
            foreach (var grid in grids.Values.ToArray())
            {
                grid.UpdateAdjacentBugCount();
            }

            foreach (var grid in gridsAddedThisUpdate)
            {
                grid.UpdateAdjacentBugCount();
            }

            gridsAddedThisUpdate.Clear();

            // Now, update the infestation state across all now existing levels.
            foreach (var grid in grids.Values.ToArray())
            {
                grid.UpdateInfestedState();
            }
        }

        public GridceptionSpec Spec { get; }

        /// <summary>
        /// Get/Create the level within the current level (+1)
        /// </summary>
        public Gridception? GetOrCreateLevelWithin(int currentLevel, bool currentLevelIsInfested) =>
            GetOrCreateLevel(currentLevel + 1, currentLevelIsInfested);

        /// <summary>
        /// Get/Create the level containing the current level (-1).
        /// </summary>
        public Gridception? GetOrCreateContainingLevel(int currentLevel, bool currentLevelIsInfested) =>
            GetOrCreateLevel(currentLevel - 1, currentLevelIsInfested);

        private Gridception? GetOrCreateLevel(int requiredLevel, bool sourceLevelIsInfested)
        {
            if (grids.TryGetValue(requiredLevel, out var grid))
            {
                return grid;
            }

            // Note: only CREATE the next level if this current level has any bugs, i.e. there is no need, and its stops us getting in an endless loop
            if (!sourceLevelIsInfested)
            {
                return null;
            }

            const bool isInfested = false; // Initially, no other levels contain bugs
            var newTiles = initialTiles.Select(x => new Tile(x.Position, isInfested));
            grid = new Gridception(requiredLevel, this, Spec, newTiles);
            grids.Add(requiredLevel, grid);
            gridsAddedThisUpdate.Add(grid);
            return grid;
        }

        public string Render()
        {
            var buffer = new StringBuilder();
            foreach (var grid in grids.Select(grid => grid.Value).Where(grid => grid.IsInfested()))
            {
                buffer.AppendLine($"Depth {grid.Level}:");
                buffer.AppendLine(grid.Render());
                buffer.AppendLine();
            }

            return buffer.ToString().TrimEnd();
        }

        public static GridceptionManager Load(string input)
        {
            var tiles = Grid.Parse(input).ToArray();

            var bottomRight = new Vector(tiles.Max(x => x.Position.X), tiles.Max(x => x.Position.Y));

            var center = new Vector(bottomRight.X / 2, bottomRight.Y / 2);

            tiles = tiles.Where(tile => !tile.Position.Equals(center)).ToArray();

            var spec = new GridceptionSpec(center, 0, 0, bottomRight.X, bottomRight.Y);

            return new GridceptionManager(tiles, spec);
        }
    }
}
