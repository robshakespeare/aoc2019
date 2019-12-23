using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Extensions;
using Common.IntCodes;

namespace AoC.Day7
{
    public class Day7Solver : SolverReadAllText
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override long? SolvePart1(string inputProgram)
        {
            var phaseSettings = (..5).ToArray();
            return Solve(inputProgram, phaseSettings);
        }

        public override long? SolvePart2(string inputProgram)
        {
            var phaseSettings = (5..10).ToArray();
            return Solve(inputProgram, phaseSettings);
        }

        private long Solve(string inputProgram, int[] phaseSettings) =>
            GetAllPossibleCombinations(phaseSettings)
                .Select(phaseSettingSequence => ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoop(inputProgram, phaseSettingSequence))
                .OrderByDescending(finalOutputSignal => finalOutputSignal)
                .First();

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

        #region Signalling, PhaseSettingSequences and FeedbackLoops

        public long ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoop(string inputProgram, int[] phaseSettingSequence)
        {
            var deviceSignalConnectors = phaseSettingSequence.Select(phaseSetting => new PhaseSignalConnector(phaseSetting))
                .ToArray();

            deviceSignalConnectors.First().SetNextValue(0); // Seed input signal of the first device

            var finalResults = new long[phaseSettingSequence.Length];

            Parallel.ForEach(
                deviceSignalConnectors.Select((connector, index) => (connector, index)),
                device =>
                {
                    var nextDeviceIndex = device.index + 1 == deviceSignalConnectors.Length ? 0 : device.index + 1;
                    var nextDeviceConnector = deviceSignalConnectors[nextDeviceIndex];

                    var result = intCodeComputer.ParseAndEvaluate(inputProgram, device.connector.ReceiveNextValue, nextDeviceConnector.SetNextValue);

                    if (result.LastOutputValue == null)
                    {
                        throw new InvalidOperationException("Invalid IntCodeComputer result, expected a LastOutputValue.");
                    }

                    finalResults[device.index] = result.LastOutputValue.Value;
                });

            return finalResults.Last();
        }

        public class SignalConnector
        {
            private readonly AutoResetEvent waitHandle = new AutoResetEvent(false);

            private long nextValue;

            public virtual long ReceiveNextValue()
            {
                waitHandle.WaitOne();
                return nextValue;
            }

            public void SetNextValue(long value)
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

            public override long ReceiveNextValue()
            {
                if (phaseSettingUsed)
                {
                    return base.ReceiveNextValue();
                }

                phaseSettingUsed = true;
                return phaseSetting;
            }
        }

        #endregion
    }
}
