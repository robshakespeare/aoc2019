using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day16
{
    public class SignalCleaner
    {
        public const int DefaultNumberOfPhases = 100;

        private readonly PatternGenerator patternGenerator = new PatternGenerator();

        public string Clean(string inputSignal, int numOfPhases = DefaultNumberOfPhases)
        {
            var signal = ParseInputSignal(inputSignal);

            var resultSignal = Clean(signal, numOfPhases);

            return string.Join("", resultSignal.Take(8));
        }

        public static IReadOnlyList<int> ParseInputSignal(string inputSignal) =>
            inputSignal.Trim().ToCharArray()
                .Select(c => int.Parse(c.ToString()))
                .ToReadOnlyArray();

        private IEnumerable<int> Clean(
            IReadOnlyList<int> signal,
            int numOfPhases = DefaultNumberOfPhases)
        {
            var patterns = patternGenerator.GeneratePatterns(signal.Count);
            return Enumerable
                .Range(0, numOfPhases)
                .Aggregate(
                    signal,
                    (currentSignal, _) => patterns
                        .Select(pattern => RunPattern(currentSignal, pattern))
                        .ToReadOnlyArray());
        }

        private static int RunPattern(IReadOnlyList<int> signal, IEnumerable<(int value, int index)> pattern) =>
            Math.Abs(pattern.Select(p => signal[p.index] * p.value).Sum() % 10);
    }
}
