namespace Day12
{
    public class Moon
    {
        public Moon(Vector position)
        {
            Position = position;
            Velocity = new Vector();
        }

        public Vector Position { get; set; }

        public Vector Velocity { get; set; }

        public long CalculateTotalEnergyForMoon()
        {
            var potentialEnergy = Position.SumAbsoluteValuesInVector();
            var kineticEnergy = Velocity.SumAbsoluteValuesInVector();

            return potentialEnergy * kineticEnergy;
        }

        public override string ToString() => $"pos={Position}, vel={Velocity}";

        ////public ulong GetState() => ((ulong)Position.GetHashCode() + int.MaxValue) | (ulong)Velocity.GetHashCode();

        public int GetState()
        {
            unchecked
            {
                return (Position.GetHashCode() * 397) ^ Velocity.GetHashCode();
            }
        }
    }
}
