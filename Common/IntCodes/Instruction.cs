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
        private long GetParamRaw(int paramIndex)
        {
            var absoluteParamIndex = CurrentInstructionPointer + 1 + paramIndex; // Note: +1 for reading the opCode
            NewInstructionPointer = Math.Max(NewInstructionPointer, absoluteParamIndex + 1); // Note: +1 to send to next unprocessed instruction
            return IntCodeState[absoluteParamIndex];
        }

        // rs-todo: need better names, so obvious which is for write index, and which is for reading, ooo, even better, have write and read methods!!
        /// <summary>
        /// Gets the parameter at the specified zero-based index, relative to this instruction, applying parameter modes to retrieve the actual value.
        /// Updates the `NewInstructionPointer` to include the number of parameters that have been read.
        /// </summary>
        public long GetParamUsingMode(int paramIndex) // rs-todo: rename to GetParameterValue
        {
            var rawValue = GetParamRaw(paramIndex);

            var parameterMode = paramIndex < parameterModes.Length
                ? parameterModes[paramIndex]
                : ParameterMode.Positional; // Positional is the default, if no mode is provided at this `paramIndex`

            // Immediate means the value is what is used
            // Positional means its value is the address
            return parameterMode switch
                {
                ParameterMode.Immediate => rawValue,
                ParameterMode.Positional => IntCodeState[rawValue],
                ParameterMode.Relative => IntCodeState[IntCodeState.RelativeBase + rawValue],
                _ => throw new InvalidOperationException("Invalid ParameterMode: " + new { parameterMode, CurrentInstructionPointer, paramIndex })
                };
        }

        // GetIntCodeReferencedByParameter(int paramIndex)
        // SetIntCodeReferencedByParameter(int paramIndex, long intCodeValue)
        // 
        // StoreValueAtAddressReferencedByParameterValue
        // WriteValueToAddressReferencedByParameterValue
        // WriteToAddressReferencedByParameterValue
        public void SetParameterValue(int paramIndex, long value) // rs-todo: hmm, should these not go in IntCodeState, so can never set a value without using paramModes?? At the mo, IntCodeState[this] setter allows bypass of paramMode!!
        {
            var rawValue = GetParamRaw(paramIndex);

            var parameterMode = paramIndex < parameterModes.Length
                ? parameterModes[paramIndex]
                : ParameterMode.Positional; // Positional is the default, if no mode is provided at this `paramIndex`

            var storageIndex = parameterMode switch
                {
                ParameterMode.Positional => rawValue,
                ParameterMode.Relative => IntCodeState.RelativeBase + rawValue,
                ParameterMode.Immediate => throw new NotSupportedException("Parameters that an instruction writes to should never be in immediate mode"),
                _ => throw new InvalidOperationException("Invalid ParameterMode: " + new { parameterMode, CurrentInstructionPointer, paramIndex })
                };

            IntCodeState[storageIndex] = value;
            //// Parameters that an instruction writes to will never be in immediate mode
            //if (parameterMode == ParameterMode.Immediate)
            //{
            //    parameterMode
            //}
        }
    }
}
