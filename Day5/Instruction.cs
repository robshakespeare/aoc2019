using System;

namespace Day5
{
    public class Instruction
    {
        private readonly ParameterMode[] parameterModes;

        public int[] IntCodes { get; }
        public int OpCode { get; }
        public int CurrentInstructionPointer { get; }

        public Instruction(int opCode, int[] intCodes, ParameterMode[] parameterModes, int instructionPointer)
        {
            OpCode = opCode;
            IntCodes = intCodes;
            this.parameterModes = parameterModes;
            CurrentInstructionPointer = instructionPointer;
        }

        public int NewInstructionPointer { get; private set; }

        /// <summary>
        /// Gets the parameter at the specified zero-based index, relative to this instruction.
        /// Updates the `NewInstructionPointer` to include the number of parameters that have been read.
        /// </summary>
        public int GetParam(int paramIndex)
        {
            var absoluteParamIndex = CurrentInstructionPointer + 1 + paramIndex; // Note: +1 for reading the opCode
            NewInstructionPointer = Math.Max(NewInstructionPointer, absoluteParamIndex + 1); // Note: +1 to send to next unprocessed instruction
            return IntCodes[absoluteParamIndex];
        }

        /// <summary>
        /// Gets the parameter at the specified zero-based index, relative to this instruction, applying parameter modes to retrieve the actual value.
        /// Updates the `NewInstructionPointer` to include the number of parameters that have been read.
        /// </summary>
        public int GetParamUsingMode(int paramIndex)
        {
            var rawValue = GetParam(paramIndex);

            var parameterMode = paramIndex < parameterModes.Length
                ? parameterModes[paramIndex]
                : ParameterMode.Positional; // Positional is the default, if no mode is provided at this `paramIndex`

            // Immediate means the value is what is used
            // Positional means its value is the address
            return parameterMode switch
                {
                ParameterMode.Immediate => rawValue,
                ParameterMode.Positional => IntCodes[rawValue],
                _ => throw new InvalidOperationException("Invalid ParameterMode: " + new { parameterMode, CurrentInstructionPointer, paramIndex })
                };
        }
    }
}
