using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Day22
{
    public class ShuffleProcessParser
    {
        public (InstructionType instruction, int operand)[] Parse(string completeShuffleProcessInput)
        {
            IEnumerable<(InstructionType instruction, int operand)> Enumerate()
            {
                var reader = new StringReader(completeShuffleProcessInput);
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "deal into new stack")
                    {
                        yield return (InstructionType.DealIntoNewStack, default);
                    }
                    else if (line.StartsWith("cut "))
                    {
                        yield return (InstructionType.Cut, Convert.ToInt32(new string(line.Skip("cut ".Length).ToArray())));
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

            return Enumerate().ToArray();
        }
    }
}
