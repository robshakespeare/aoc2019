using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using static AoC.Day16.SignalCleaner;

namespace AoC.Day16
{
    public class SignalCleanerPart2
    {
        public string RealClean(string inputSignal)
        {
            const int inputRepetitions = 10000;
            
            var signal = ParseInputSignal(inputSignal);
            var messageOffset = int.Parse(string.Join("", signal.Take(7)));

            var realSignal = Enumerable.Repeat(signal, inputRepetitions).SelectMany(x => x).ToReadOnlyArray();

            if (messageOffset < Math.Ceiling(realSignal.Count / 2m))
            {
                throw new NotSupportedException("This method only supports decoding of signals whose message offset is in the second half of the real signal.");
            }

            var signalToProcess = realSignal.Skip(messageOffset).ToReadOnlyArray();

            Console.WriteLine("signalToProcess: " + string.Join("", signalToProcess));

            var resultSignal = RealClean(signalToProcess);

            return string.Join("", resultSignal.Take(8));
        }

        private static IEnumerable<int> RealClean(
            IReadOnlyList<int> signal,
            int numOfPhases = DefaultNumberOfPhases) =>
            Enumerable
                .Range(0, numOfPhases)
                .Aggregate(signal, EvaluateSinglePhasePart2);

        private static IReadOnlyList<int> EvaluateSinglePhasePart2(IReadOnlyList<int> currentSignal, int phaseIndex)
        {
            var result = new int[currentSignal.Count];
            var runningTotal = 0;
            foreach (var element in currentSignal.Select((value, index) => (value, index)).Reverse())
            {
                runningTotal += element.value;

                result[element.index] = runningTotal % 10;
            }

            return result.ToReadOnlyArray();
        }
    }
}
