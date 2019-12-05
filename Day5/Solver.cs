using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5
{
    // Yuk.  This has become horrible code!  Needs refactoring to do it properly!
    public class Solver
    {
        public static int[] LoadIntCodes() =>
            File.ReadAllText("input.txt")
                .Split(',')
                .Select(int.Parse)
                .ToArray();

        public (int opCode, bool param1IsImmediate, bool param2IsImmediate, bool param3IsImmediate) ParseFullOpCode(int fullOpCode)
        {
            fullOpCode = Math.Abs(fullOpCode); // Ignore negative numbers

            var opCodeChars = fullOpCode.ToString().PadLeft(5, '0').ToCharArray();

            if (opCodeChars.Length > 5)
            {
                throw new InvalidOperationException("Invalid length of opCode: " + fullOpCode);
            }

            var param3IsImmediate = opCodeChars[^5] == '1';
            var param2IsImmediate = opCodeChars[^4] == '1';
            var param1IsImmediate = opCodeChars[^3] == '1';

            var opCode = int.Parse($"{opCodeChars[^2]}{opCodeChars[^1]}");

            return (opCode, param1IsImmediate, param2IsImmediate, param3IsImmediate);
        }

        public int ProcessIntCodes(int[] intCodes, int inputValue, Stack<int> outputs, int instructionPointer = 0)
        {
            var fullOpCode = intCodes[instructionPointer];
            var (opCode, param1IsImmediate, param2IsImmediate, _) = ParseFullOpCode(fullOpCode);

            if (opCode == 99)
            {
                return intCodes[0];
            }

            int newInstructionPointer;

            switch (opCode)
            {
                case 3:
                {
                    // opCode 3 takes a single integer as input and saves it to the address given by its only parameter.
                    // For example, the instruction 3,50 would take an input value and store it at address 50.
                    var addressIndex = intCodes[instructionPointer + 1];
                    intCodes[addressIndex] = inputValue;

                    newInstructionPointer = instructionPointer + 2;
                    break;
                }

                case 4:
                {
                    // opCode 4 outputs the value of its only parameter.
                    // For example, the instruction 4,50 would output the value at address 50.
                    var addressIndex = intCodes[instructionPointer + 1];
                    outputs.Push(intCodes[addressIndex]);

                    newInstructionPointer = instructionPointer + 2;
                    break;
                }

                case 5:
                case 6:
                {
                    // Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to
                    // the value from the second parameter. Otherwise, it does nothing.

                    // Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to
                    // the value from the second parameter. Otherwise, it does nothing.

                    var param1 = intCodes[instructionPointer + 1];
                    var param2 = intCodes[instructionPointer + 2];

                    param1 = param1IsImmediate ? param1 : intCodes[param1];
                    param2 = param2IsImmediate ? param2 : intCodes[param2];

                    switch (opCode)
                    {
                        case 5 when param1 != 0:
                        case 6 when param1 == 0:
                            newInstructionPointer = param2;
                            break;

                        default:
                            newInstructionPointer = instructionPointer + 3;
                            break;
                    }

                    break;
                }

                case 7:
                case 8:
                {
                    // Opcode 7 is less than: if the first parameter is less than the second parameter,
                    // it stores 1 in the position given by the third parameter. Otherwise, it stores 0.

                    // Opcode 8 is equals: if the first parameter is equal to the second parameter,
                    // it stores 1 in the position given by the third parameter. Otherwise, it stores 0.

                    var param1 = intCodes[instructionPointer + 1];
                    var param2 = intCodes[instructionPointer + 2];
                    var param3 = intCodes[instructionPointer + 3];

                    param1 = param1IsImmediate ? param1 : intCodes[param1];
                    param2 = param2IsImmediate ? param2 : intCodes[param2];

                    var storageIndex = param3;

                    var result = opCode switch
                        {
                        7 when param1 < param2 => 1,
                        8 when param1 == param2 => 1,
                        _ => 0
                        };

                    intCodes[storageIndex] = result;

                    newInstructionPointer = instructionPointer + 4;
                    break;
                }

                default:
                {
                    // Immediate means the value is what used
                    // Positional means its value is the address

                    var param1 = intCodes[instructionPointer + 1];
                    var param2 = intCodes[instructionPointer + 2];
                    var param3 = intCodes[instructionPointer + 3];

                    param1 = param1IsImmediate ? param1 : intCodes[param1];
                    param2 = param2IsImmediate ? param2 : intCodes[param2];

                    var storageIndex = param3;

                    var result = opCode switch
                        {
                        1 => param1 + param2,
                        2 => param1 * param2,
                        _ => throw new InvalidOperationException("Invalid opCode: " + opCode)
                        };

                    intCodes[storageIndex] = result;

                    newInstructionPointer = instructionPointer + 4;
                    break;
                }
            }

            return ProcessIntCodes(intCodes, inputValue, outputs, newInstructionPointer);
        }

        public int Solve()
        {
            var intCodes = LoadIntCodes();
            // Note: const int airConditionerUnitId = 1;
            const int thermalRadiatorControllerId = 5;
            const int inputValue = thermalRadiatorControllerId;
            var outputs = new Stack<int>();

            ProcessIntCodes(intCodes, inputValue, outputs);

            var diagnosticCode = outputs.Pop();

            return diagnosticCode;
        }
    }
}
