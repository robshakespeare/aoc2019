using System;
using System.Collections.Generic;

namespace Day14
{
    public class ChemicalQuantityMap
    {
        private readonly Dictionary<string, int> map = new Dictionary<string, int>();

        public int this[string chemical] => map[chemical];

        public void IncreaseQuantity(string chemical, in int quantity)
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

        public void DepleteIfAvailable(string chemical, ref int quantityRequired)
        {
            if (map.TryGetValue(chemical, out var currentQuantity))
            {
                var amountDepleted = Math.Min(quantityRequired, currentQuantity);

                map[chemical] = currentQuantity - amountDepleted;

                quantityRequired -= amountDepleted;
            }
        }

        //// <returns>Returns the amount depleted, if any.</returns>
        ////public int DepleteIfAvailable(string chemical, int quantityRequired)
        ////{
        ////    if (map.TryGetValue(chemical, out var currentQuantity))
        ////    {
        ////        var amountDepleted = Math.Min(quantityRequired, currentQuantity);

        ////        map[chemical] = currentQuantity - amountDepleted;
        ////        return amountDepleted;
        ////    }

        ////    return 0;
        ////}
    }
}
