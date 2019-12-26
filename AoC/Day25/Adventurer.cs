using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;

namespace AoC.Day25
{
    public class Adventurer
    {
        public const string SecurityCheckpointRoomName = "Security Checkpoint";
        public const string PressureSensitiveFloorRoomName = "Pressure-Sensitive Floor";

        private static readonly Regex RoomNameRegex = new Regex("(?<roomName>.+) ==", RegexOptions.Compiled);
        private static readonly Regex BulletTextRegex = new Regex(@"- (?<bulletText>[^\n\r]+)", RegexOptions.Compiled);

        private readonly AsciiComputer asciiComputer = new AsciiComputer();
        private readonly StringBuilder outputBuffer = new StringBuilder();

        private readonly Dictionary<string, RoomMemory> roomMemories = new Dictionary<string, RoomMemory>();
        private readonly List<string> currentInventory = new List<string>();

        private RoomMemory? currentRoom;
        private string homeRoom = "";
        private bool interactive; // Starts as false, input should be auto-decided. True means input should be requested from actual user, i.e. me!
        private Direction? lastAttemptedDirection;
        private List<string> itemsHere = new List<string>();
        private bool visitedSecurityCheckpointRoom;
        private readonly Queue<string> nextInstructionQueue = new Queue<string>(); 

        public string GoOnAdventure(string inputProgram)
        {
            asciiComputer.ParseAndEvaluate(inputProgram, ProvideInput, ConsumeOutput);
            return outputBuffer.ToString();
        }

        private string ProvideInput()
        {
            Update();

            var input = (interactive
                ? Console.ReadLine()
                : DecideNextInstruction()) ?? throw new InvalidOperationException("null input is not valid!");

            if (Direction.IsDirection(input))
            {
                lastAttemptedDirection = Direction.Get(input);
            }

            if (input.StartsWith("take "))
            {
                var item = input.Substring("take ".Length);
                currentInventory.Add(item);
                itemsHere.Remove(item);
            }

            if (input.StartsWith("drop "))
            {
                var item = input.Substring("drop ".Length);
                currentInventory.Remove(item);
                itemsHere.Add(item);
            }

            return input;
        }

        private void Update()
        {
            var output = outputBuffer.ToString().Trim();
            outputBuffer.Clear();

            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine(output);

                foreach (var outputChunk in output.Split(new[] { "== " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    ParseOutputChunk(outputChunk);
                }
            }
        }

        private void ParseOutputChunk(string output)
        {
            var bullets = BulletTextRegex.Matches(output)
                .Select(x => new
                {
                    bulletText = x.Groups["bulletText"].Value,
                    isDirection = Direction.IsDirection(x.Groups["bulletText"].Value)
                }).ToArray();

            var possibleExists = bullets.Where(x => x.isDirection).Select(x => Direction.Get(x.bulletText)).ToArray();

            // If the output contains any "non direction bullets", that means we have a new set of items that we can take
            // So refresh our list of possible items to take.
            if (bullets.Any(x => !x.isDirection))
            {
                itemsHere = new List<string>(bullets.Where(x => !x.isDirection).Select(x => x.bulletText));
            }

            // If we have just moved in to a room...
            var roomMatch = RoomNameRegex.Match(output);
            if (roomMatch.Success)
            {
                var roomName = roomMatch.Groups["roomName"].Value;

                if (homeRoom == "")
                {
                    homeRoom = roomName;
                }

                if (!roomMemories.ContainsKey(roomName))
                {
                    roomMemories.Add(
                        roomName,
                        new RoomMemory(roomName, output, lastAttemptedDirection?.OppositeDirection, possibleExists));
                }

                currentRoom = roomMemories[roomName];
            }
        }

        private void EnqueueDropItemCommands(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                nextInstructionQueue.Enqueue($"drop {item}");
            }
        }

        private void EnqueueTakeItemCommands(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                nextInstructionQueue.Enqueue($"take {item}");
            }
        }

        private void EnqueueAllSubsetsOfItems(IList<string>[] subsetsToTry)
        {
            var instructionToVisitPressureSensitiveRoom =
                currentRoom?.PossibleExits.First(x => x.Name != currentRoom.LeaveByDirection?.Name).Name
                ?? throw new InvalidOperationException("Failed to get instruction to visit Pressure Sensitive Room");

            EnqueueDropItemCommands(currentInventory);

            foreach (var subset in subsetsToTry)
            {
                EnqueueTakeItemCommands(subset);
                nextInstructionQueue.Enqueue(instructionToVisitPressureSensitiveRoom);
                EnqueueDropItemCommands(subset);
            }
        }

        private string DecideNextInstruction()
        {
            if (nextInstructionQueue.Count > 0)
            {
                return nextInstructionQueue.Dequeue();
            }

            if (currentRoom?.Name == SecurityCheckpointRoomName)
            {
                if (!visitedSecurityCheckpointRoom)
                {
                    visitedSecurityCheckpointRoom = true;

                    EnqueueAllSubsetsOfItems(currentInventory
                            .Where(x => x != "spool of cat6" &&
                                        x != "hologram" &&
                                        x != "shell")
                            .ToArray()
                            .Subsets()
                            .ToArray());
                }
            }

            while (itemsHere?.Count > 0)
            {
                var itemToTake = itemsHere.First();
                itemsHere.Remove(itemToTake);

                if (!IsBadItem(itemToTake))
                {
                    return $"take {itemToTake}";
                }
            }

            if (currentRoom?.RemainingExitsToTry.Count > 0)
            {
                return currentRoom.RemainingExitsToTry.Dequeue().Name;
            }

            if (currentRoom?.LeaveByDirection != null)
            {
                Console.WriteLine(">> No more doors to try, leaving by the door we entered by...");
                return currentRoom.LeaveByDirection.Name;
            }

            return SwitchToManualMode();
        }

        private string SwitchToManualMode()
        {
            Console.WriteLine(">> Don't know what to do next! Switching to interactive mode, please provide next command:");
            interactive = true;
            return ProvideInput();
        }

        private void ConsumeOutput(string line)
        {
            outputBuffer.AppendLine(line);
        }

        private static bool IsBadItem(string itemName) =>
            itemName switch
                {
                "giant electromagnet" => true,
                "infinite loop" => true,
                "molten lava" => true,
                "photons" => true,
                "escape pod" => true,
                _ => false
                };
    }
}
