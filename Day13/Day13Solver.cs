using System.Linq;
using Common;
using Common.IntCodes;
using MoreLinq;

namespace Day13
{
    public class Day13Solver : SolverReadAllText
    {
        public static void Main() => new Day13Solver().Run();

        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string input)
        {
            var result = intCodeComputer.ParseAndEvaluate(input);

            return result.Outputs
                .Batch(3)
                .Select(batch => batch.ElementAt(2))
                .Select(EnumUtil.Parse<TileType>)
                .Count(tileType => tileType == TileType.Block);
        }

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
