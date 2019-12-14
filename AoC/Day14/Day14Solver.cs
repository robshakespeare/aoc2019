using Common;

namespace Day14
{
    public class Day14Solver : SolverReadAllText
    {
        public override long? SolvePart1(string input) => NanoFactory.Create(input).CalculateOreRequired(1);

        private const long OneTrillion = 1000000000000L;

        public override long? SolvePart2(string input)
        {
            var nanoFactory = NanoFactory.Create(input);

            // Use a kind of divide and conquer approach to narrow down to the answer for:
            // Given 1 trillion ORE, what is the maximum amount of FUEL you can produce?
            // Find the last fuel figure that can be used, before the ORE required exceeds 1 trillion.

            var range = FindInitialFuelRange(nanoFactory);

            var maxFuelCanProduce = (long?)null;

            while(maxFuelCanProduce == null)
            {
                var midPointFuel = range.startFuel + (range.endFuel - range.startFuel) / 2;

                if (midPointFuel == range.startFuel)
                {
                    // Our range of fuel is now 1 or 2 wide, so we have our match
                    maxFuelCanProduce = midPointFuel;
                }

                // Find where the 1 trillion ORE lies, to see whether we should continue to search within top or bottom half
                var ore = nanoFactory.CalculateOreRequired(midPointFuel);

                if (ore == OneTrillion)
                {
                    // extreme case, where this fuel produces exactly 1 trillion, neither less, we have our answer
                    maxFuelCanProduce = midPointFuel;
                }
                else if (ore > OneTrillion)
                {
                    // Ore is above OneTrillion, can exclude top half
                    range = (range.startFuel, range.startOre, midPointFuel, ore);
                }
                else
                {
                    // Ore is below OneTrillion, can exclude bottom half
                    range = (midPointFuel, ore, range.endFuel, range.endOre);
                }
            }

            return maxFuelCanProduce.Value;
        }

        private static (long startFuel, long startOre, long endFuel, long endOre) FindInitialFuelRange(NanoFactory nanoFactory)
        {
            // Keep increasing the top until the amount of ore exceeds 1 trillion
            const long fuelStep = 1000000L;

            var fuel = 0L;
            var prevFuel = 0L;
            var prevOreResult = 0L;
            var oreResult = 0L;

            while (oreResult < OneTrillion)
            {
                prevFuel = fuel;
                prevOreResult = oreResult;

                fuel += fuelStep;
                oreResult = nanoFactory.CalculateOreRequired(fuel);
            }

            return (prevFuel, prevOreResult, fuel, oreResult);
        }
    }
}
