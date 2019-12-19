using System;
using System.Collections.Generic;

namespace AoC.Day18
{
    public class Grid
    {
        private readonly Dictionary<Vector, char> grid;
        private readonly Dictionary<char, Vector> doors;
        private readonly Dictionary<char, Vector> keys;

        public Grid(
            Dictionary<Vector, char> grid,
            Dictionary<char, Vector> doors,
            Dictionary<char, Vector> keys)
        {
            this.grid = grid;
            this.doors = doors;
            this.keys = keys;
        }

        public int NumberOfKeysRemaining => keys.Count;

        public string KeysRemaining => string.Join("", keys.Keys);

        public bool IsKey(Vector location, out char key) => grid.TryGetValue(location, out key) && IsKey(key);

        public static bool IsKey(char piece) => piece >= 'a' && piece <= 'z';

        public static bool IsDoor(char piece) => piece >= 'A' && piece <= 'Z';

        public static bool IsOpenSpace(char piece) => piece == '.';

        public bool IsAvailableLocation(Vector location) => grid.TryGetValue(location, out var piece) && IsOpenSpace(piece) || IsKey(piece);

        public char PickUpKeyAndUnlockDoor(Vector keyLocation)
        {
            var key = grid[keyLocation];

            if (!IsKey(key))
            {
                throw new InvalidOperationException("Attempted to pick up key from location with no key");
            }

            keys.Remove(key);
            grid[keyLocation] = '.';

            UnlockDoor(key);

            return key;
        }

        private void UnlockDoor(char key)
        {
            var door = key.ToString().ToUpperInvariant()[0];

            // Note that the final key, might not have a corresponding door!
            if (doors.TryGetValue(door, out var doorLocation))
            {
                doors.Remove(door);

                if (!IsDoor(grid[doorLocation]))
                {
                    throw new InvalidOperationException($"Expected door at {doorLocation}, but was not door!");
                }

                grid[doorLocation] = '.';
            }
        }

        public Grid CloneWithRefToGrid() => new Grid(
            new Dictionary<Vector, char>(grid),
            new Dictionary<char, Vector>(doors),
            new Dictionary<char, Vector>(keys));
    }
}
