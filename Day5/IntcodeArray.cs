using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day5
{
    public class IntcodeArray
    {
        private readonly int[] intCodes;
        private int instructionPointer;
        private ParameterMode[] currentParameterModes;

        public IntcodeArray(int[] intCodes)
        {
            this.intCodes = intCodes;
            this.instructionPointer = 0;
        }

        private int Read() => intCodes[instructionPointer++];

        /// <summary>
        /// Reads the next code, as an "Op Code", and advances the instruction pointer by 1.
        /// </summary>
        /// <returns></returns>
        public (int opCode, ParameterMode[] parameterModes) ReadOpCode()
        {
            var fullOpCode = Read();
            fullOpCode = Math.Abs(fullOpCode); // Ignore negative numbers

            var opCodeChars = fullOpCode.ToString().PadLeft(5, '0').ToCharArray().Reverse().ToArray();

            var opCode = int.Parse($"{string.Join("", opCodeChars.Take(2))}");

            // Store parameters modes, so read param methods can apply param mode if necessary
            currentParameterModes = opCodeChars.Skip(2)
                .Select(x => x == '1' ? ParameterMode.Immediate : ParameterMode.Positional)
                .ToArray();

            return (opCode, currentParameterModes);
        }

        public int ReadParamWithMode()
    }
}
