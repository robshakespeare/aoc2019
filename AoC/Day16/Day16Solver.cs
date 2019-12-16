using Common;

namespace AoC.Day16
{
    public class Day16Solver : Solver<string, string?, string?>
    {
        public Day16Solver() : base(new InputLoaderReadAllText(16))
        {
        }

        public override string? SolvePart1(string inputSignal) => new SignalCleaner().Clean(inputSignal);

        public override string? SolvePart2(string inputSignal) => new SignalCleanerPart2().RealClean(inputSignal);
    }
}
