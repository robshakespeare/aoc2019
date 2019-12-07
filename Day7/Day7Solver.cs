using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Extensions;
using Common.IntCodes;

namespace Day7
{
    public class Day7Solver : SolverReadAllText
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override int? SolvePart1(string inputProgram)
        {
            var phaseSettings = (..5).ToArray();

            return GetAllPossibleCombinations(phaseSettings)
                .Select(phaseSettingSequence => TryPhaseSettingSequence(inputProgram, phaseSettingSequence))
                .OrderByDescending(finalOutputSignal => finalOutputSignal)
                .First();
        }

        public int[][] GetAllPossibleCombinations(int[] values) =>
            values.Length == 1
                ? new[] {values}
                : values
                    .SelectMany((value, index) =>
                    {
                        var otherValues = values.Where((_, otherIndex) => otherIndex != index).ToArray();
                        return GetAllPossibleCombinations(otherValues)
                            .Select(x => x.Prepend(value))
                            .Select(x => x.ToArray());
                    })
                    .ToArray();

        public int TryPhaseSettingSequence(string inputProgram, int[] phaseSettingSequence)
        {
            var signal = 0;

            foreach (var phaseSetting in phaseSettingSequence)
            {
                var result = intCodeComputer.ParseAndEvaluate(inputProgram, phaseSetting, signal);

                if (result.LastOutputValue == null)
                {
                    throw new InvalidOperationException("Invalid IntCodeComputer result, expected a LastOutputValue.");
                }

                signal = result.LastOutputValue.Value;
            }

            return signal;
        }

        public override int? SolvePart2(string inputProgram)
        {
            var phaseSettings = (5..10).ToArray();

            return GetAllPossibleCombinations(phaseSettings)
                .Select(phaseSettingSequence => TryPhaseSettingSequenceWithFeedbackLoop(inputProgram, phaseSettingSequence))
                .OrderByDescending(finalOutputSignal => finalOutputSignal)
                .First();
        }

        public class SignalConnector
        {
            private readonly AutoResetEvent waitHandle = new AutoResetEvent(false);

            private int nextValue; 

            public virtual int ReceiveNextValue()
            {
                waitHandle.WaitOne();
                return nextValue;
            }

            public void SetNextValue(int value)
            {
                nextValue = value;
                waitHandle.Set();
            }
        }

        public class PhaseSignalConnector : SignalConnector
        {
            private readonly int phaseSetting;
            private bool phaseSettingUsed;

            public PhaseSignalConnector(int phaseSetting)
            {
                this.phaseSetting = phaseSetting;
            }

            public override int ReceiveNextValue()
            {
                if (phaseSettingUsed)
                {
                    return base.ReceiveNextValue();
                }

                phaseSettingUsed = true;
                return phaseSetting;
            }
        }

        public int TryPhaseSettingSequenceWithFeedbackLoop(string inputProgram, int[] phaseSettingSequence)
        {
            var amplifierSignalConnectors = phaseSettingSequence.Select(phaseSetting => new PhaseSignalConnector(phaseSetting))
                .ToArray();

            amplifierSignalConnectors.First().SetNextValue(0); // Seed input signal of the first amplifier

            var finalResults = new int[phaseSettingSequence.Length];

            Parallel.ForEach(
                amplifierSignalConnectors.Select((connector, index) => (connector, index)),
                amp =>
                {
                    var nextAmpIndex = amp.index + 1 == amplifierSignalConnectors.Length ? 0 : amp.index + 1;
                    var nextAmpConnector = amplifierSignalConnectors[nextAmpIndex];

                    var result = intCodeComputer.ParseAndEvaluateWithSignalling(inputProgram, amp.connector.ReceiveNextValue, nextAmpConnector.SetNextValue);

                    if (result.LastOutputValue == null)
                    {
                        throw new InvalidOperationException("Invalid IntCodeComputer result, expected a LastOutputValue.");
                    }

                    finalResults[amp.index] = result.LastOutputValue.Value;
                });

            return finalResults.Last();
        }
    }
}
