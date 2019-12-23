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
            var network = new Network();
            var computers = Enumerable.Range(0, 50)
                .Select(address =>
                {
                    var intCodeState = intCodeStateInput.CloneWithReset();
                    var nic = network.AddNIC(address);

                    return (intCodeState, nic);
                })
                .ToArray();

            while (network.NAT.LastPacketReceived == null)
            {
                foreach (var (intCodeState, nic) in computers)
                {
                    IntCodeComputer.EvaluateNextInstruction(intCodeState, nic.DequeueIncomingValue, nic.EnqueueOutgoingValue);
                }
            }

            return network.NAT.LastPacketReceived.Value.y;
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
