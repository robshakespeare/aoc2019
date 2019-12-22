using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Day22
{
    public class ShuffleProcessParser
    {
        public IEnumerable<(InstructionType instruction, int operand)> Parse(string completeShuffleProcessInput)
        {
            var reader = new StringReader(completeShuffleProcessInput);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "deal into new stack")
                {
                    yield return (InstructionType.DealIntoNewStack, default);
                }
                else if (line.StartsWith("cut -"))
                {
                    yield return (InstructionType.CutNegative, Convert.ToInt32(new string(line.Skip("cut -".Length).ToArray())));
                }
                else if (line.StartsWith("cut "))
                {
                    yield return (InstructionType.CutPositive, Convert.ToInt32(new string(line.Skip("cut ".Length).ToArray())));
                }
                else if (line.StartsWith("deal with increment "))
                {
                    yield return (InstructionType.DealWithIncrement, Convert.ToInt32(new string(line.Skip("deal with increment ".Length).ToArray())));
                }
                else
                {
                    throw new InvalidOperationException("Invalid input line: " + line);
                }
            }
        }
    }
}
