using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.IntCodes
{
    public class IntCodeComputer
    {
        public IntCodeState Parse(string inputProgram, Func<long> getNextInputValue, Action<long>? onNewOutputValue) => new IntCodeState(
            inputProgram.Split(',')
                .Select(long.Parse)
                .ToArray(),
            getNextInputValue,
            onNewOutputValue);

        public IntCodeState ParseAndEvaluate(string inputProgram, params long[]? inputValues)
        {
            var inputValuesQueue = new Queue<long>(inputValues ?? Array.Empty<long>());
            return ParseAndEvaluateWithSignalling(inputProgram, () => inputValuesQueue.Dequeue(), null);
        }

        public IntCodeState ParseAndEvaluateWithSignalling(string inputProgram, Func<long> receiveInputValue, Action<long>? sendOutputValue)
        {
            var intCodeState = Parse(inputProgram, receiveInputValue, sendOutputValue);

            Instruction instruction;
            while ((instruction = intCodeState.ReadNextInstruction()).OpCode != 99)
            {
                var gotoInstructionPointer = EvalInstruction(instruction);

                intCodeState.InstructionPointer = gotoInstructionPointer ?? instruction.NewInstructionPointer;
            }

            return intCodeState;
        }

        private static long? EvalInstruction(Instruction instruction) =>
            instruction.OpCode switch
                {
                1 => EvalMathInstruction(instruction),
                2 => EvalMathInstruction(instruction),
                3 => EvalInputInstruction(instruction),
                4 => EvalOutputInstruction(instruction),
                5 => EvalJumpInstruction(instruction),
                6 => EvalJumpInstruction(instruction),
                7 => EvalRelationalInstruction(instruction),
                8 => EvalRelationalInstruction(instruction),
                9 => EvalUpdateRelativeBaseOffset(instruction),
                _ => throw new InvalidOperationException("Invalid opCode: " + instruction.OpCode)
                };

        private static long? EvalMathInstruction(Instruction instruction)
        {
            var param1 = instruction.GetParamUsingMode(0);
            var param2 = instruction.GetParamUsingMode(1);

            ////var storageIndex = instruction.GetParam(2);

            var result = instruction.OpCode switch
                {
                1 => param1 + param2,
                2 => param1 * param2,
                _ => throw new InvalidOperationException("Invalid Math opCode: " + instruction.OpCode)
                };

            instruction.SetParameterValue(2, result); // rs-todo: this needs doing better!!
            return null;
        }

        private static long? EvalInputInstruction(Instruction instruction)
        {
            // opCode 3 takes a single integer as input and saves it to the address given by its only parameter.
            // For example, the instruction 3,50 would take an input value and store it at address 50.
            var inputValue = instruction.IntCodeState.GetNextInputValue();

            ////var storageIndex = instruction.GetParam(0);
            ////instruction.IntCodeState[storageIndex] = inputValue;
            instruction.SetParameterValue(0, inputValue); // rs-todo: this needs doing better!!
            return null;
        }

        private static long? EvalOutputInstruction(Instruction instruction)
        {
            // opCode 4 outputs the value of its only parameter.
            // For example, the instruction 4,50 would output the value at address 50.
            var outputValue = instruction.GetParamUsingMode(0);
            instruction.IntCodeState.Outputs.Add(outputValue);
            instruction.IntCodeState.OnNewOutputValue?.Invoke(outputValue);
            return null;
        }

        private static long? EvalJumpInstruction(Instruction instruction)
        {
            // Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to
            // the value from the second parameter. Otherwise, it does nothing.

            // Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to
            // the value from the second parameter. Otherwise, it does nothing.

            var param1 = instruction.GetParamUsingMode(0);
            var param2 = instruction.GetParamUsingMode(1);

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

            var param1 = instruction.GetParamUsingMode(0);
            var param2 = instruction.GetParamUsingMode(1);

            ////var storageIndex = instruction.GetParam(2);

            var result = instruction.OpCode switch
                {
                7 when param1 < param2 => 1,
                8 when param1 == param2 => 1,
                _ => 0
                };

            ////instruction.IntCodeState[storageIndex] = result;
            instruction.SetParameterValue(2, result); // rs-todo: this needs doing better!!
            return null;
        }

        private static long? EvalUpdateRelativeBaseOffset(Instruction instruction)
        {
            var param1 = instruction.GetParamUsingMode(0);
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
