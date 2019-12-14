using System;
using System.Collections.Generic;

namespace Day14
{
    public class ChemicalQuantityMap
    {
        private readonly Dictionary<string, long> map = new Dictionary<string, long>();

        public long this[string chemical] => map[chemical];

        public void IncreaseQuantity(string chemical, in long quantity)
        {
            if (map.TryGetValue(chemical, out var currentQuantity))
            {
                map[chemical] = currentQuantity + quantity;
            }
            else
            {
                map.Add(chemical, quantity);
            }
        }

        public void DepleteIfAvailable(string chemical, ref long quantityRequired)
        {
            if (map.TryGetValue(chemical, out var currentQuantity))
            {
                var amountDepleted = Math.Min(quantityRequired, currentQuantity);

                map[chemical] = currentQuantity - amountDepleted;

                quantityRequired -= amountDepleted;
            }
        }
    }
}
