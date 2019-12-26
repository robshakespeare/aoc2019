using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day25
{
    public class Direction
    {
        private readonly Lazy<Direction> oppositeDirection;

        private Direction(string name, string oppositeDirectionName)
        {
            Name = name;
            oppositeDirection = new Lazy<Direction>(() => Get(oppositeDirectionName));
        }

        public string Name { get; }

        public Direction OppositeDirection => oppositeDirection.Value;

        public override string ToString() => Name;

        public static readonly Direction North = new Direction("north", "south");
        public static readonly Direction South = new Direction("south", "north");
        public static readonly Direction West = new Direction("west", "east");
        public static readonly Direction East = new Direction("east", "west");

        public static readonly Dictionary<string, Direction> All = new[] {North, East, South, West}.ToDictionary(x => x.Name);

        public static Direction Get(string directionName) => All[directionName];

        public static bool IsDirection(string text) => All.ContainsKey(text);
    }
}
