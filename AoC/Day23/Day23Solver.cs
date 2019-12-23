using System.Linq;
using Common;
using Common.IntCodes;

namespace AoC.Day23
{
    public class Day23Solver : SolverReadAllText
    {
        private static readonly IntCodeComputer IntCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string inputProgram)
        {
            var intCodeStateInput = IntCodeComputer.Parse(inputProgram);

            long? result = null;
            var network = new Network((address, x, y) =>
            {
                if (address == 255)
                {
                    result = y;
                }
            });

            var computers = Enumerable.Range(0, 50)
                .Select(address =>
                {
                    var intCodeState = intCodeStateInput.CloneWithReset();
                    var nic = network.AddNIC(address);

                    return (intCodeState, nic);
                })
                .ToArray();

            while (result == null)
            {
                foreach (var (intCodeState, nic) in computers)
                {
                    IntCodeComputer.EvaluateNextInstruction(intCodeState, nic.DequeueIncomingValue, nic.EnqueueOutgoingValue);
                }
            }

            return result;
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
