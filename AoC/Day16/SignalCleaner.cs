using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day16
{
    public class SignalCleaner
    {
        private readonly PatternGenerator patternGenerator = new PatternGenerator();

        public string Clean(string inputSignal, int numOfPhases)
        {
            var signal = inputSignal.Trim().ToCharArray()
                .Select(c => int.Parse(c.ToString()))
                .ToReadonlyArray();

            var patterns = patternGenerator.GeneratePatterns(inputSignal.Length);

            var resultSignal = Enumerable
                .Range(0, numOfPhases)
                .Aggregate(
                    signal,
                    (currentSignal, _) => patterns
                        .Select(pattern => RunPattern(currentSignal, pattern))
                        .ToReadonlyArray());

            return string.Join("", resultSignal.Take(8));
        }

        private static int RunPattern(IEnumerable<int> signal, IReadOnlyList<int> pattern) =>
            Math.Abs(signal.Select((value, index) => value * pattern[index]).Sum() % 10);
    }
}
