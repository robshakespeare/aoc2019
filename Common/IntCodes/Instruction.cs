using System;

namespace Common.IntCodes
{
    public class Instruction
    {
        private readonly ParameterMode[] parameterModes;

        public IntCodeState IntCodeState { get; }
        public long OpCode { get; }
        public long CurrentInstructionPointer { get; }

        public Instruction(long opCode, IntCodeState intCodeState, ParameterMode[] parameterModes, long instructionPointer)
        {
            OpCode = opCode;
            IntCodeState = intCodeState;
            this.parameterModes = parameterModes;
            CurrentInstructionPointer = instructionPointer;
        }

        public long NewInstructionPointer { get; private set; }

        /// <summary>
        /// Gets the parameter at the specified zero-based index, relative to this instruction.
        /// Updates the `NewInstructionPointer` to include the number of parameters that have been read.
        /// </summary>
        private long GetRawParameterValue(int paramIndex)
        {
            var absoluteParamIndex = CurrentInstructionPointer + 1 + paramIndex; // Note: +1 for reading the opCode
            NewInstructionPointer = Math.Max(NewInstructionPointer, absoluteParamIndex + 1); // Note: +1 to send to next unprocessed instruction
            return IntCodeState[absoluteParamIndex];
        }

        private ParameterMode GetParameterMode(int paramIndex) =>
            paramIndex < parameterModes.Length
                ? parameterModes[paramIndex]
                : ParameterMode.Positional; // Positional is the default, if no mode is provided at this `paramIndex`

        /// <summary>
        /// Gets the IntCode value at the address referenced by the parameter at the specified zero-based index.
        /// The parameter index is relative to the current instruction pointer.
        /// The parameter's value is de-referenced to the address by applying the corresponding parameter mode for that param. 
        /// Updates the `NewInstructionPointer` to include the number of parameters that have been read.
        /// </summary>
        public long GetIntCodeReferencedByParameter(int paramIndex)
        {
            var rawValue = GetRawParameterValue(paramIndex);
            var parameterMode = GetParameterMode(paramIndex);

            return parameterMode switch
                {
                ParameterMode.Immediate => rawValue,
                ParameterMode.Positional => IntCodeState[rawValue],
                ParameterMode.Relative => IntCodeState[IntCodeState.RelativeBase + rawValue],
                _ => throw new InvalidOperationException("Invalid ParameterMode: " + new { parameterMode, CurrentInstructionPointer, paramIndex })
                };
        }

        /// <summary>
        /// Sets the IntCode to the specified value at the address referenced by the parameter at the specified zero-based index.
        /// The parameter index is relative to the current instruction pointer.
        /// The parameter's value is de-referenced to the address by applying the corresponding parameter mode for that param. 
        /// Updates the `NewInstructionPointer` to include the number of parameters that have been read.
        /// </summary>
        public void SetIntCodeReferencedByParameter(int paramIndex, long intCodeValue)
        {
            var rawValue = GetRawParameterValue(paramIndex);
            var parameterMode = GetParameterMode(paramIndex);

            var storageIndex = parameterMode switch
                {
                ParameterMode.Positional => rawValue,
                ParameterMode.Relative => IntCodeState.RelativeBase + rawValue,
                ParameterMode.Immediate => throw new NotSupportedException("Parameters that an instruction writes to should never be in immediate mode"),
                _ => throw new InvalidOperationException("Invalid ParameterMode: " + new { parameterMode, CurrentInstructionPointer, paramIndex })
                };

            IntCodeState[storageIndex] = intCodeValue;
        }
    }
}
