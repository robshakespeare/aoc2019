using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5
{
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

        public int ProcessIntCodes(int[] intCodes, int inputValue, Stack<int> outputs, int instructionPointer)
        {
            var fullOpCode = intCodes[instructionPointer];
            var (opCode, param1IsImmediate, param2IsImmediate, param3IsImmediate) = ParseFullOpCode(fullOpCode);

            if (opCode == 99)
            {
                return intCodes[0];
            }

            int instructionPointerIncrease;

            switch (opCode)
            {
                case 3:
                {
                    // opCode 3 takes a single integer as input and saves it to the address given by its only parameter.
                    // For example, the instruction 3,50 would take an input value and store it at address 50.
                    var addressIndex = intCodes[instructionPointer + 1];
                    intCodes[addressIndex] = inputValue;

                    instructionPointerIncrease = 2;
                    break;
                }

                case 4:
                {
                    // opCode 4 outputs the value of its only parameter.
                    // For example, the instruction 4,50 would output the value at address 50.
                    var addressIndex = intCodes[instructionPointer + 1];
                    outputs.Push(intCodes[addressIndex]);

                    instructionPointerIncrease = 2;
                    break;
                }

                default:
                {
                    // Immediate means the value is what used
                    // Positional means its value is the address

                    var param1 = intCodes[instructionPointer + 1];
                    var param2 = intCodes[instructionPointer + 2];
                    var param3 = intCodes[instructionPointer + 3];

                    var left = param1IsImmediate ? param1 : intCodes[param1];
                    var right = param2IsImmediate ? param2 : intCodes[param2];

                    var storageIndex = param3; // Parameters that an instruction writes to will never be in immediate mode
                    ////var storageIndex = param3IsImmediate ? param3 : intCodes[param3];

                    var result = opCode switch
                        {
                        1 => left + right,
                        2 => left * right,
                        _ => throw new InvalidOperationException("Invalid opCode: " + opCode)
                        };

                    intCodes[storageIndex] = result;

                    instructionPointerIncrease = 4;
                    break;
                }
            }

            return ProcessIntCodes(intCodes, inputValue, outputs, instructionPointer + instructionPointerIncrease);
        }

        public int Solve()
        {
            var intCodes = LoadIntCodes();
            const int inputValue = 1;
            var outputs = new Stack<int>();

            ProcessIntCodes(intCodes, inputValue, outputs, 0);

            var diagnosticCode = outputs.Pop();

            ////if (outputs.Any(output => output != 0))
            ////{
            ////    throw new InvalidOperationException(
            ////        "Non-zero outputs mean that a function is not working correctly; check the instructions that were run before the output instruction to see which one failed");
            ////}

            return diagnosticCode;
        }
    }
}
