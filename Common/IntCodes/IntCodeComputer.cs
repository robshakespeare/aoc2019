using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.IntCodes
{
    public class IntCodeComputer
    {
        // Func<long> getNextInputValue, Action<long>? onNewOutputValue

        /// <summary>
        /// Parses the specified IntCode computer program, and creates a state object to represent the computer in its initial state.
        /// </summary>
        public IntCodeState Parse(string inputProgram) => new IntCodeState(
            inputProgram.Split(',')
                .Select(long.Parse)
                .ToArray());

        /// <summary>
        /// Parses and then evaluates the specified IntCode computer state until it halts.
        /// </summary>
        public IntCodeState ParseAndEvaluate(string inputProgram, params long[]? inputValues)
        {
            var inputValuesQueue = new Queue<long>(inputValues ?? Array.Empty<long>());
            return ParseAndEvaluateWithSignalling(inputProgram, () => inputValuesQueue.Dequeue(), null);
        }

        /// <summary>
        /// Parses and then evaluates the specified IntCode computer state until it halts, using input and output redirection.
        /// </summary>
        public IntCodeState ParseAndEvaluateWithSignalling(string inputProgram, Func<long> receiveInputValue, Action<long>? sendOutputValue) =>
            Evaluate(Parse(inputProgram), receiveInputValue, sendOutputValue);

        /// <summary>
        /// Evaluates the specified IntCode computer state until it halts.
        /// </summary>
        public IntCodeState Evaluate(IntCodeState intCodeState, Func<long> getNextInputValue, Action<long>? onNewOutputValue)
        {
            while (EvaluateNextInstruction(intCodeState, getNextInputValue, onNewOutputValue))
            {
            }
            return intCodeState;
        }

        /// <summary>
        /// Evaluates the specified IntCode computer state until it halts.
        /// </summary>
        public IntCodeState EvaluateUntilHaltedOrInputNeeded(IntCodeState intCodeState, Action<long>? onNewOutputValue = null)
        {
            while (intCodeState.ReadNextInstruction().OpCode != 3
                   && EvaluateNextInstruction(intCodeState, () => throw new InvalidOperationException(), onNewOutputValue))
            {
            }
            return intCodeState;
        }

        /// <summary>
        /// Evaluates the next instruction in the specified IntCode computer state.
        /// Returns true if the program should continue to be evaluated, otherwise if the program has now halted returns false.
        /// </summary>
        public bool EvaluateNextInstruction(IntCodeState intCodeState, Func<long> getNextInputValue, Action<long>? onNewOutputValue = null)
        {
            var instruction = intCodeState.ReadNextInstruction();
            if (instruction.OpCode == 99)
            {
                intCodeState.Halted = true;
                return false;
            }

            var gotoInstructionPointer = EvalInstruction(instruction, getNextInputValue, onNewOutputValue);
            intCodeState.InstructionPointer = gotoInstructionPointer ?? instruction.NewInstructionPointer;
            return true;
        }

        private static long? EvalInstruction(Instruction instruction, Func<long> getNextInputValue, Action<long>? onNewOutputValue) =>
            instruction.OpCode switch
                {
                1 => EvalMathInstruction(instruction),
                2 => EvalMathInstruction(instruction),
                3 => EvalInputInstruction(instruction, getNextInputValue),
                4 => EvalOutputInstruction(instruction, onNewOutputValue),
                5 => EvalJumpInstruction(instruction),
                6 => EvalJumpInstruction(instruction),
                7 => EvalRelationalInstruction(instruction),
                8 => EvalRelationalInstruction(instruction),
                9 => EvalUpdateRelativeBaseOffset(instruction),
                _ => throw new InvalidOperationException("Invalid opCode: " + instruction.OpCode)
                };

        private static long? EvalMathInstruction(Instruction instruction)
        {
            var param1 = instruction.GetIntCodeReferencedByParameter(0);
            var param2 = instruction.GetIntCodeReferencedByParameter(1);

            var result = instruction.OpCode switch
                {
                1 => param1 + param2,
                2 => param1 * param2,
                _ => throw new InvalidOperationException("Invalid Math opCode: " + instruction.OpCode)
                };

            instruction.SetIntCodeReferencedByParameter(2, result);
            return null;
        }

        private static long? EvalInputInstruction(Instruction instruction, Func<long> getNextInputValue)
        {
            // opCode 3 takes a single integer as input and saves it to the address given by its only parameter.
            // For example, the instruction 3,50 would take an input value and store it at address 50.
            var inputValue = getNextInputValue();

            instruction.SetIntCodeReferencedByParameter(0, inputValue);
            return null;
        }

        private static long? EvalOutputInstruction(Instruction instruction, Action<long>? onNewOutputValue)
        {
            // opCode 4 outputs the value of its only parameter.
            // For example, the instruction 4,50 would output the value at address 50.
            var outputValue = instruction.GetIntCodeReferencedByParameter(0);
            instruction.IntCodeState.Outputs.Add(outputValue);
            onNewOutputValue?.Invoke(outputValue);
            return null;
        }

        private static long? EvalJumpInstruction(Instruction instruction)
        {
            // Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to
            // the value from the second parameter. Otherwise, it does nothing.

            // Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to
            // the value from the second parameter. Otherwise, it does nothing.

            var param1 = instruction.GetIntCodeReferencedByParameter(0);
            var param2 = instruction.GetIntCodeReferencedByParameter(1);

            switch (instruction.OpCode)
            {
                case 5 when param1 != 0:
                case 6 when param1 == 0:
                    return param2; // i.e. goto this Instruction Pointer

                default:
                    return null;
            }
        }

        private static long? EvalRelationalInstruction(Instruction instruction)
        {
            // Opcode 7 is less than: if the first parameter is less than the second parameter,
            // it stores 1 in the position given by the third parameter. Otherwise, it stores 0.

            // Opcode 8 is equals: if the first parameter is equal to the second parameter,
            // it stores 1 in the position given by the third parameter. Otherwise, it stores 0.

            var param1 = instruction.GetIntCodeReferencedByParameter(0);
            var param2 = instruction.GetIntCodeReferencedByParameter(1);

            var result = instruction.OpCode switch
                {
                7 when param1 < param2 => 1,
                8 when param1 == param2 => 1,
                _ => 0
                };

            instruction.SetIntCodeReferencedByParameter(2, result);
            return null;
        }

        private static long? EvalUpdateRelativeBaseOffset(Instruction instruction)
        {
            var param1 = instruction.GetIntCodeReferencedByParameter(0);
            instruction.IntCodeState.RelativeBase += param1;
            return null;
        }

        #region Signalling, PhaseSettingSequences and FeedbackLoops

        public long ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoop(string inputProgram, int[] phaseSettingSequence)
        {
            var deviceSignalConnectors = phaseSettingSequence.Select(phaseSetting => new PhaseSignalConnector(phaseSetting))
                .ToArray();

            deviceSignalConnectors.First().SetNextValue(0); // Seed input signal of the first device

            var finalResults = new long[phaseSettingSequence.Length];

            Parallel.ForEach(
                deviceSignalConnectors.Select((connector, index) => (connector, index)),
                device =>
                {
                    var nextDeviceIndex = device.index + 1 == deviceSignalConnectors.Length ? 0 : device.index + 1;
                    var nextDeviceConnector = deviceSignalConnectors[nextDeviceIndex];

                    var result = ParseAndEvaluateWithSignalling(inputProgram, device.connector.ReceiveNextValue, nextDeviceConnector.SetNextValue);

                    if (result.LastOutputValue == null)
                    {
                        throw new InvalidOperationException("Invalid IntCodeComputer result, expected a LastOutputValue.");
                    }

                    finalResults[device.index] = result.LastOutputValue.Value;
                });

            return finalResults.Last();
        }

        public class SignalConnector
        {
            private readonly AutoResetEvent waitHandle = new AutoResetEvent(false);

            private long nextValue;

            public virtual long ReceiveNextValue()
            {
                waitHandle.WaitOne();
                return nextValue;
            }

            public void SetNextValue(long value)
            {
                nextValue = value;
                waitHandle.Set();
            }
        }

        public class PhaseSignalConnector : SignalConnector
        {
            private readonly int phaseSetting;
            private bool phaseSettingUsed;

            public PhaseSignalConnector(int phaseSetting)
            {
                this.phaseSetting = phaseSetting;
            }

            public override long ReceiveNextValue()
            {
                if (phaseSettingUsed)
                {
                    return base.ReceiveNextValue();
                }

                phaseSettingUsed = true;
                return phaseSetting;
            }
        }

        #endregion
    }
}
