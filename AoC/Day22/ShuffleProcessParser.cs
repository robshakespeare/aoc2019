using System;
using System.Linq;
using Common.Extensions;

namespace AoC.Day22
{
    public class ShuffleProcessParser
    {
        public (Technique technique, int operand)[] Parse(string shuffleProcessInstructions) =>
            shuffleProcessInstructions
                .ReadAllLines()
                .Select(line => line switch
                {
                    _ when line == "deal into new stack" => (Technique.DealIntoNewStack, default),
                    _ when line.StartsWith("cut ") => (Technique.Cut, Convert.ToInt32(line.Substring("cut ".Length))),
                    _ when line.StartsWith("deal with increment ") => (Technique.DealWithIncrement, Convert.ToInt32(line.Substring("deal with increment ".Length))),
                    _ => throw new InvalidOperationException("Invalid input line: " + line)
                })
                .ToArray();
    }
}
