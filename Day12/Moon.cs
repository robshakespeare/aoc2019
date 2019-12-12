namespace Day12
{
    public class Moon
    {
        public Moon(Vector position)
        {
            Position = position;
            Velocity = new Vector(0 ,0, 0);
        }

        public Vector Position { get; set; }

        public Vector Velocity { get; set; }
    }
}
