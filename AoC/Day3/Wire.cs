using System.Collections.Generic;

namespace Day3
{
    public class Wire
    {
        public Wire(IEnumerable<Vector> movements)
        {
            Movements = movements;
        }

        public IEnumerable<Vector> Movements { get; }
    }
}