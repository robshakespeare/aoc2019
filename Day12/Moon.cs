using static Day12.Vector;

namespace Day12
{
    public class Moon
    {
        public Moon(Vector position)
        {
            Position = position;
            Velocity = new Vector(0, 0, 0);

            InitialPosition = new Vector(position[X], position[Y], position[Z]);
            InitialVelocity = new Vector(0, 0, 0);
        }

        public Vector Position { get; set; }

        public Vector Velocity { get; set; }

        public Vector InitialPosition { get; set; }

        public Vector InitialVelocity { get; set; }

        public long CalculateTotalEnergyForMoon()
        {
            var potentialEnergy = Position.SumAbsoluteValuesInVector();
            var kineticEnergy = Velocity.SumAbsoluteValuesInVector();

            return potentialEnergy * kineticEnergy;
        }

        public override string ToString() => $"pos={Position}, vel={Velocity}";
    }
}
