using System;
using System.Collections.Generic;

namespace AoC.Day23
{
    public class Network
    {
        private readonly Action<long, long, long> onSend;
        private readonly Dictionary<long, NetworkInterfaceController> nics = new Dictionary<long, NetworkInterfaceController>();

        public Network(Action<long, long, long> onSend)
        {
            this.onSend = onSend;
        }

        public NetworkInterfaceController AddNIC(long address)
        {
            var nic = new NetworkInterfaceController(address, this);
            nics.Add(address, nic);
            return nic;
        }

        public void Send(in long address, long x, long y)
        {
            if (nics.TryGetValue(address, out var destination))
            {
                destination.Receive(x, y);
            }

            onSend(address, x, y);
        }
    }
}
