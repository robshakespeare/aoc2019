using System;
using System.Linq;

namespace Common.IntCodes
{
    public class IntCodeComputer
    {
        public IntCodeState Parse(string input, int inputSystemId) => new IntCodeState(
            input.Split(',')
                .Select(int.Parse)
                .ToArray(),
            inputSystemId);

        public (IntCodeState intCodeState, int? diagnosticCode) ParseAndEvaluate(string input, int inputSystemId)
        {
            var intCodeState = Parse(input, inputSystemId);

            Instruction instruction;
            while ((instruction = intCodeState.ReadNextInstruction()).OpCode != 99)
            {
                var gotoInstructionPointer = EvalInstruction(instruction);

                intCodeState.InstructionPointer = gotoInstructionPointer ?? instruction.NewInstructionPointer;
            }

            return (intCodeState, intCodeState.Outputs.Any() ? intCodeState.Outputs.Peek() : (int?)null);
        }

        private static int? EvalInstruction(Instruction instruction) =>
            instruction.OpCode switch
                {
                1 => EvalMathInstruction(instruction),
                2 => EvalMathInstruction(instruction),
                3 => EvalTakeInstruction(instruction),
                4 => EvalOutputInstruction(instruction),
                5 => EvalJumpInstruction(instruction),
                6 => EvalJumpInstruction(instruction),
                7 => EvalRelationalInstruction(instruction),
                8 => EvalRelationalInstruction(instruction),
                _ => throw new InvalidOperationException("Invalid opCode: " + instruction.OpCode)
                };

        private static int? EvalMathInstruction(Instruction instruction)
        {
            var param1 = instruction.GetParamUsingMode(0);
            var param2 = instruction.GetParamUsingMode(1);
            var param3 = instruction.GetParam(2);

            var storageIndex = param3;

            var result = instruction.OpCode switch
                {
                1 => param1 + param2,
                2 => param1 * param2,
                _ => throw new InvalidOperationException("Invalid Math opCode: " + instruction.OpCode)
                };

            instruction.IntCodeState[storageIndex] = result;
            return null;
        }

        private static int? EvalTakeInstruction(Instruction instruction)
        {
            // opCode 3 takes a single integer as input and saves it to the address given by its only parameter.
            // For example, the instruction 3,50 would take an input value and store it at address 50.
            var addressIndex = instruction.GetParam(0);
            instruction.IntCodeState[addressIndex] = instruction.IntCodeState.InputSystemId;
            return null;
        }

        private static int? EvalOutputInstruction(Instruction instruction)
        {
            // opCode 4 outputs the value of its only parameter.
            // For example, the instruction 4,50 would output the value at address 50.
            var addressIndex = instruction.GetParam(0);
            instruction.IntCodeState.Outputs.Push(instruction.IntCodeState[addressIndex]);
            return null;
        }

        private static int? EvalJumpInstruction(Instruction instruction)
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

        private static int? EvalRelationalInstruction(Instruction instruction)
        {
            // Opcode 7 is less than: if the first parameter is less than the second parameter,
            // it stores 1 in the position given by the third parameter. Otherwise, it stores 0.

            // Opcode 8 is equals: if the first parameter is equal to the second parameter,
            // it stores 1 in the position given by the third parameter. Otherwise, it stores 0.

            var param1 = instruction.GetParamUsingMode(0);
            var param2 = instruction.GetParamUsingMode(1);
            var param3 = instruction.GetParam(2);

            var storageIndex = param3;

            var result = instruction.OpCode switch
                {
                7 when param1 < param2 => 1,
                8 when param1 == param2 => 1,
                _ => 0
                };

            instruction.IntCodeState[storageIndex] = result;
            return null;
        }
    }
}