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
            var signal = ParseInputSignal(inputSignal);

            var resultSignal = Clean(signal, numOfPhases);

            return string.Join("", resultSignal.Take(8));
        }

        private static IReadOnlyList<int> ParseInputSignal(string inputSignal) =>
            inputSignal.Trim().ToCharArray()
                .Select(c => int.Parse(c.ToString()))
                .ToReadonlyArray();

        public IReadOnlyList<int> Clean(
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
                        .ToReadonlyArray());
        }

        private static int RunPattern(IReadOnlyList<int> signal, IEnumerable<(int value, int index)> pattern) =>
            Math.Abs(pattern.Select(p => signal[p.index] * p.value).Sum() % 10);

        public string RealClean(string inputSignal)
        {
            const int inputRepetitions = 10000;
            
            var signal = ParseInputSignal(inputSignal);
            var messageOffset = int.Parse(string.Join("", signal.Take(7)));

            var fullSignal = Enumerable.Repeat(signal, inputRepetitions).SelectMany(x => x).ToReadonlyArray();

            var signalToProcess = fullSignal.Skip(messageOffset).ToReadonlyArray();

            var resultSignal = Clean2(signalToProcess);

            return string.Join("", resultSignal.Take(8));
        }

        public IReadOnlyList<int> Clean2(
            IReadOnlyList<int> signal,
            int numOfPhases = DefaultNumberOfPhases) =>
            Enumerable
                .Range(0, numOfPhases)
                .Aggregate(
                    signal,
                    (currentSignal, _) =>
                        Enumerable
                            .Range(0, signal.Count)
                            .Select(numToSkip => currentSignal.Skip(numToSkip).Sum() % 10)
                            .ToReadonlyArray());
    }
}
