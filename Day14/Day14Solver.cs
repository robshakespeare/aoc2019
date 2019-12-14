using Common;

namespace Day14
{
    public class Day14Solver : SolverReadAllText
    {
        public static void Main() => new Day14Solver().Run();

        public override long? SolvePart1(string input) => NanoFactory.Create(input).CalculateOreRequired(1);

        public override long? SolvePart2(string input)
        {
            return base.SolvePart2(input);
        }
    }
}
