using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.IntCodes
{
    public class IntCodeState
    {
        private readonly int[] intCodes;

        public IntCodeState(int[] intCodes, int inputSystemId)
        {
            InputSystemId = inputSystemId;
            this.intCodes = intCodes;
            InstructionPointer = 0;
            Outputs = new Stack<int>();
        }

        public int InputSystemId { get; }

        public int InstructionPointer { get; set; }

        public Stack<int> Outputs { get; }

        /// <summary>
        /// Gets or sets the int code at the specified index.
        /// </summary>
        public int this[int index]
        {
            get => intCodes[index];
            set => intCodes[index] = value;
        }

        /// <summary>
        /// Reads the next code, as an "Op Code", and advances the instruction pointer by 1.
        /// </summary>
        /// <returns></returns>
        public Instruction ReadNextInstruction()
        {
            var fullOpCode = intCodes[InstructionPointer];
            fullOpCode = Math.Abs(fullOpCode); // Ignore negative numbers

            var opCode = fullOpCode % 100; // Get just the tens and units

            var parameterModes = fullOpCode.ToString()
                .Reverse()
                .Skip(2)
                .Select(chr => chr == '1' ? ParameterMode.Immediate : ParameterMode.Positional)
                .ToArray();

            return new Instruction(opCode, this, parameterModes, InstructionPointer);
        }
    }
}
