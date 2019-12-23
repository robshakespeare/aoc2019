using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day23
{
    public class NAT
    {
        private readonly Network network;
        private int idleCounter;

        public NAT(Network network)
        {
            this.network = network;
            idleCounter = 0;
        }

        public (long x, long y)? LastPacketReceived { get; private set; }

        public List<(long x, long y)> PacketsReleased { get; } = new List<(long x, long y)>();

        public void Receive((long x, long y) packet)
        {
            LastPacketReceived = packet;
        }

        public void Update()
        {
            // If all computers have empty incoming packet queues and are continuously trying to receive packets without sending packets, the network is considered idle.
            var idle = network.Nics.All(nic => nic.IncomingIsEmpty);

            idleCounter = idle ? idleCounter + 1 : 0;

            // If network is idle, NAT should release the last packet it received to address 0
            if (idleCounter >= 650)
            {
                ReleaseLastPacketReceived();
            }
        }

        public void ReleaseLastPacketReceived()
        {
            var lastPacketReceived = LastPacketReceived ?? throw new InvalidOperationException("No NAT packet has been received yet");
            PacketsReleased.Add(lastPacketReceived);
            network.Send(0, lastPacketReceived);
        }
    }
}
