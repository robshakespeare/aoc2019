using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day16
{
    public class SignalCleaner
    {
        private const int DefaultNumberOfPhases = 100;

        private readonly PatternGenerator patternGenerator = new PatternGenerator();

        public string Clean(string inputSignal, int numOfPhases = DefaultNumberOfPhases)
        {
            var patterns = patternGenerator.GeneratePatterns(inputSignal.Length);
            var signal = ParseInputSignal(inputSignal);

            var resultSignal = Clean(signal, numOfPhases, patterns);

            return string.Join("", resultSignal.Take(8));
        }

        private static IReadOnlyList<int> ParseInputSignal(string inputSignal) =>
            inputSignal.Trim().ToCharArray()
                .Select(c => int.Parse(c.ToString()))
                .ToReadonlyArray();

        public IReadOnlyList<int> Clean(IReadOnlyList<int> signal, int numOfPhases, IReadOnlyCollection<IReadOnlyCollection<(int value, int index)>> patterns) =>
            Enumerable
                .Range(0, numOfPhases)
                .Aggregate(
                    signal,
                    (currentSignal, _) => patterns
                        .Select(pattern => RunPattern(currentSignal, pattern))
                        .ToReadonlyArray());

        public string RealClean(string inputSignal)
        {
            const int inputRepetitions = 10000;

            var patterns = patternGenerator.GeneratePatterns(inputSignal.Length);
            var signal = ParseInputSignal(inputSignal);

            var messageOffset = int.Parse(string.Join("", signal.Take(7)));

            var resultSignal = Enumerable
                .Range(0, inputRepetitions)
                .Aggregate(
                    signal,
                    (currentSignal, _) => Clean(currentSignal, DefaultNumberOfPhases, patterns));

            return string.Join("", resultSignal.Skip(messageOffset).Take(8));
        }

        private static int RunPattern(IReadOnlyList<int> signal, IEnumerable<(int value, int index)> pattern) =>
            Math.Abs(pattern.Select(p => signal[p.index] * p.value).Sum() % 10);
    }
}
