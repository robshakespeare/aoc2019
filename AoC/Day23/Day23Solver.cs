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
            var (network, computers) = SetupNetwork(inputProgram);

            while (network.NAT.LastPacketReceived == null)
            {
                foreach (var (intCodeState, nic) in computers)
                {
                    IntCodeComputer.EvaluateNextInstruction(intCodeState, nic.DequeueIncomingValue, nic.EnqueueOutgoingValue);
                }
            }

            return network.NAT.LastPacketReceived.Value.y;
        }

        public override long? SolvePart2(string inputProgram)
        {
            var (network, computers) = SetupNetwork(inputProgram);

            while (network.NAT.PacketsReleased.Count < 2 ||
                   network.NAT.PacketsReleased.Last().y != network.NAT.PacketsReleased[network.NAT.PacketsReleased.Count - 2].y)
            {
                foreach (var (intCodeState, nic) in computers)
                {
                    IntCodeComputer.EvaluateNextInstruction(intCodeState, nic.DequeueIncomingValue, nic.EnqueueOutgoingValue);
                }

                network.NAT.Update();
            }

            return network.NAT.PacketsReleased.Last().y;
        }

        private static (Network network, (IntCodeState intCodeState, NetworkInterfaceController nic)[] computers) SetupNetwork(string inputProgram)
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
            return (network, computers);
        }
    }
}
