using System.Collections.Generic;

namespace AoC.Day23
{
    public class NetworkInterfaceController
    {
        private readonly Network network;
        private readonly Queue<long> outgoing = new Queue<long>();
        private readonly Queue<long> incoming = new Queue<long>();

        public NetworkInterfaceController(in long address, in Network network)
        {
            this.network = network;
            Address = address;
            incoming.Enqueue(address);
        }

        public long Address { get; }

        public void Receive((long x, long y) packet)
        {
            incoming.Enqueue(packet.x);
            incoming.Enqueue(packet.y);
        }

        public bool IncomingIsEmpty => incoming.Count == 0;

        public long DequeueIncomingValue() => incoming.Count > 0 ? incoming.Dequeue() : -1;

        public void EnqueueOutgoingValue(long value)
        {
            outgoing.Enqueue(value);

            if (outgoing.Count == 3)
            {
                var address = outgoing.Dequeue();
                var x = outgoing.Dequeue();
                var y = outgoing.Dequeue();

                network.Send(address, (x, y));
            }
        }
    }
}
