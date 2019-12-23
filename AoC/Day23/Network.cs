using System.Collections.Generic;

namespace AoC.Day23
{
    public class Network
    {
        private readonly Dictionary<long, NetworkInterfaceController> nics = new Dictionary<long, NetworkInterfaceController>();

        public Network()
        {
            NAT = new NAT(this);
        }

        public NAT NAT { get; }

        public IEnumerable<NetworkInterfaceController> Nics => nics.Values;

        public NetworkInterfaceController AddNIC(long address)
        {
            var nic = new NetworkInterfaceController(address, this);
            nics.Add(address, nic);
            return nic;
        }

        public void Send(in long address, (long x, long y) packet)
        {
            if (address == 255)
            {
                NAT.Receive(packet);
            }
            else if (nics.TryGetValue(address, out var destinationNic))
            {
                destinationNic.Receive(packet);
            }
        }
    }
}
