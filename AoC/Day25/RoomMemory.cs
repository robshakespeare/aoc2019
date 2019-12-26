using System.Collections.Generic;
using System.Linq;

namespace AoC.Day25
{
    public class RoomMemory
    {
        public string Name { get; }
        public string FullText { get; }
        public Direction? LeaveByDirection { get; }
        public Direction[] PossibleExits { get; }
        public Queue<Direction> RemainingExitsToTry { get; }

        public RoomMemory(string name, string fullText, Direction? leaveByDirection, Direction[] possibleExits)
        {
            Name = name;
            FullText = fullText;
            LeaveByDirection = leaveByDirection;
            PossibleExits = possibleExits;
            RemainingExitsToTry = new Queue<Direction>(
                possibleExits.Where(x => leaveByDirection == null || x.Name != leaveByDirection.Name));
        }
    }
}
