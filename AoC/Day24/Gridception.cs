using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day24
{
    public class Gridception : Grid
    {
        private readonly GridceptionManager manager;

        public Gridception(
            int level,
            GridceptionManager manager,
            GridceptionSpec spec,
            IEnumerable<Tile> tiles)
            : base(tiles)
        {
            this.manager = manager;
            Level = level;
            Spec = spec;
        }

        public override void Update() => throw new NotSupportedException("Multi-level grids cannot be updated independently!");

        public void UpdateAdjacentBugCount()
        {
            foreach (var tile in Tiles)
            {
                tile.UpdateAdjacentBugCount(this);
            }
        }

        public void UpdateInfestedState()
        {
            foreach (var tile in Tiles)
            {
                tile.UpdateInfestedState();
            }
        }

        public int Level { get; }
        public GridceptionSpec Spec { get; }

        public override int CountAdjacentBugs(Vector position)
        {
            return new[]
                {
                    MovementCommand.North.MovementVector,
                    MovementCommand.East.MovementVector,
                    MovementCommand.South.MovementVector,
                    MovementCommand.West.MovementVector
                }
                .Select(movement =>
                {
                    var newPosition = movement + position;
                    return new
                    {
                        movement,
                        position = newPosition,
                        tile = TryGetTile(newPosition)
                    };
                })
                .SelectMany(adjacentPosition =>
                {
                    if (adjacentPosition.position.Equals(Spec.Center)) // i.e. adjacent position is the center of the grid
                    {
                        // Get/Create the level within this one (+1), and then return its corresponding OUTER edges tiles
                        // Note: only CREATE the next level if this current level has any bugs, i.e. there is no need, and its stops us getting in an endless loop
                        // Double note: GET should always be done, otherwise we would never update any other levels!

                        var levelWithin = manager.GetOrCreateLevelWithin(Level, IsInfested());

                        return levelWithin != null
                            ? levelWithin.GetOuterEdgeTiles(adjacentPosition.movement)
                            : Enumerable.Empty<Tile>();
                    }

                    if (adjacentPosition.tile == null) // i.e. adjacent position is outside the grid, on the outer side
                    {
                        // Get/Create the level containing this one (-1), and then return its corresponding INNER edge tile
                        // Note: only CREATE the next level if this current level has any bugs, i.e. there is no need, and its stops us getting in an endless loop
                        // Double note: GET should always be done, otherwise we would never update any other levels!

                        var levelWithin = manager.GetOrCreateContainingLevel(Level, IsInfested());

                        return levelWithin != null
                            ? new[] {levelWithin.GetInnerEdgeTile(adjacentPosition.movement)}
                            : Enumerable.Empty<Tile>();
                    }

                    return new[] {adjacentPosition.tile};
                })
                .Count(tile => tile.IsInfested);
        }

        private Tile GetInnerEdgeTile(Vector movement)
        {
            var innerEdgePosition = Spec.Center + movement;
            return TryGetTile(innerEdgePosition) ??
                   throw new InvalidOperationException($"Inner edge position {innerEdgePosition} had no tile, at level {Level}!");
        }

        private IEnumerable<Tile> GetOuterEdgeTiles(Vector movement)
        {
            return movement switch
                {
                _ when movement.Equals(MovementCommand.North.MovementVector) => GetBottomTiles(),
                _ when movement.Equals(MovementCommand.East.MovementVector) => GetLeftTiles(),
                _ when movement.Equals(MovementCommand.South.MovementVector) => GetTopTiles(),
                _ when movement.Equals(MovementCommand.West.MovementVector) => GetRightTiles(),
                _ => throw new InvalidOperationException($"Invalid movement: {movement}")
                };
        }

        private IEnumerable<Tile> GetTiles(Vector start, Vector end, Vector movement)
        {
            var position = start;
            var done = false;
            do
            {
                yield return TryGetTile(position) ??
                             throw new InvalidOperationException($"Outer edge position {position} had no tile, at level {Level}!");

                if (position.Equals(end))
                {
                    done = true;
                }
                else
                {
                    position += movement;
                }
            } while (!done);
        }

        private IEnumerable<Tile> GetTopTiles() => GetTiles(Spec.TopLeft, Spec.TopRight, MovementCommand.East.MovementVector);

        private IEnumerable<Tile> GetBottomTiles() => GetTiles(Spec.BottomLeft, Spec.BottomRight, MovementCommand.East.MovementVector);

        private IEnumerable<Tile> GetLeftTiles() => GetTiles(Spec.TopLeft, Spec.BottomLeft, MovementCommand.South.MovementVector);

        private IEnumerable<Tile> GetRightTiles() => GetTiles(Spec.TopRight, Spec.BottomRight, MovementCommand.South.MovementVector);

        public override string Render()
        {
            var result = base.Render();
            return string.Join(
                    Environment.NewLine,
                    result.ReadAllLines()
                        .Select((line, index) => index == Spec.Center.Y
                            ? SpliceWithCenterTileMarker(line)
                            : line));
        }

        private string SpliceWithCenterTileMarker(string line)
        {
            var chars = new List<char>(line);
            chars.Insert(Spec.Center.X, '?');
            return new string(chars.ToArray());
        }
    }
}
