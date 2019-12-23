namespace AoC.Day23
{
    public class NAT
    {
        public (long x, long y)? LastPacketReceived { get; private set; }

        public void Receive((long x, long y) packet)
        {
            LastPacketReceived = packet;
        }
    }
}
