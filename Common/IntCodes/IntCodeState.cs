using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.IntCodes
{
    public class IntCodeState
    {
        private readonly SortedList<long, long> intCodes; // The Key is the INDEX of the code, and Value is the actual code.

        public IntCodeState(long[] intCodes)
            : this(IntCodeArrayToSortedList(intCodes))
        {
        }

        private static SortedList<long, long> IntCodeArrayToSortedList(long[] intCodes)
        {
            var result = new SortedList<long, long>();

            foreach (var (code, index) in intCodes.Select((code, index) => (code, index)))
            {
                result.Add(index, code);
            }

            return result;
        }

        private IntCodeState(SortedList<long, long> intCodes)
        {
            this.intCodes = intCodes;
            Halted = false;
            InstructionPointer = 0;
            RelativeBase = 0;
            Outputs = new List<long>();
        }

        public IntCodeState CloneWithReset() => new IntCodeState(new SortedList<long, long>(intCodes));

        public bool Halted { get; internal set; }

        public long InstructionPointer { get; set; }

        public long RelativeBase { get; set; }

        public List<long> Outputs { get; }

        public long? LastOutputValue => Outputs.Any() ? Outputs.Last() : (long?) null;

        /// <summary>
        /// Gets or sets the int code at the specified index.
        /// </summary>
        /// <remarks>
        /// Rather than initialise a stupidly big amount of memory, the codes memory is initialized to the size of the provided intCodes
        /// And any accessing of memory above that range is dynamically added, via the setter, or dynamically added and then retrieved, by the getter.
        /// </remarks>
        public long this[long index]
        {
            get
            {
                if (index < 0)
                {
                    throw new InvalidOperationException("It is invalid to try to access memory at a negative address");
                }

                if (intCodes.TryGetValue(index, out var value))
                {
                    return value;
                }

                return intCodes[index] = 0;
            }
            set => intCodes[index] = value;
        }

        /// <summary>
        /// Reads the next code, as an "Op Code", and advances the instruction pointer by 1.
        /// </summary>
        /// <returns></returns>
        public Instruction ReadNextInstruction()
        {
            var fullOpCode = this[InstructionPointer];
            fullOpCode = Math.Abs(fullOpCode); // Ignore negative numbers

            var opCode = fullOpCode % 100; // Get just the tens and units

            var parameterModes = fullOpCode.ToString()
                .Reverse()
                .Skip(2)
                .Select(EnumUtil.Parse<ParameterMode>)
                .ToArray();

            return new Instruction(opCode, this, parameterModes, InstructionPointer);
        }
    }
}
