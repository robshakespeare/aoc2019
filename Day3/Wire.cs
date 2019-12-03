using System.Collections.Generic;

namespace Day3
{
    public class Wire
    {
        public Wire(IEnumerable<Vector> coordinates)
        {
            Coordinates = coordinates;
        }

        public IEnumerable<Vector> Coordinates { get; }
    }
}