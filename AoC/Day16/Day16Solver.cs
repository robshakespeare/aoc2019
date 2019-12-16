using Common;

namespace AoC.Day16
{
    public class Day16Solver : Solver<string, string?, string?>
    {
        private static readonly SignalCleaner SignalCleaner = new SignalCleaner();

        public Day16Solver() : base(new InputLoaderReadAllText(16))
        {
        }

        public override string? SolvePart1(string inputSignal) => SignalCleaner.Clean(inputSignal);

        public override string? SolvePart2(string inputSignal) => SignalCleaner.RealClean(inputSignal);
    }
}
