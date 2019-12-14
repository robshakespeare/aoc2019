using System.Collections.Generic;

namespace Day6
{
    public class HierarchicalNodeEqualityComparer : IEqualityComparer<HierarchicalNode>
    {
        public bool Equals(HierarchicalNode? x, HierarchicalNode? y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public int GetHashCode(HierarchicalNode? obj) => obj?.Id?.GetHashCode() ?? 0;
    }
}
