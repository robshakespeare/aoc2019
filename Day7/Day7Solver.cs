using System;
using System.Linq;
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
                    throw new InvalidOperationException("Invalid IntCodeComputer result, expected a FinalOutput.");
                }

                signal = result.LastOutputValue.Value;
            }

            return signal;
        }

        public override int? SolvePart2(string inputProgram)
        {
            var phaseSettings = (5..10).ToArray();

            return GetAllPossibleCombinations(phaseSettings)
                .Select(phaseSettingSequence => TryPhaseSettingSequencePart2(inputProgram, phaseSettingSequence))
                .OrderByDescending(finalOutputSignal => finalOutputSignal)
                .First();
        }

        public int TryPhaseSettingSequencePart2(string inputProgram, int[] phaseSettingSequence)
        {
            var signal = 0;

            foreach (var phaseSetting in phaseSettingSequence)
            {
                var result = intCodeComputer.ParseAndEvaluate(inputProgram, phaseSetting, signal);

                if (result.LastOutputValue == null)
                {
                    throw new InvalidOperationException("Invalid IntCodeComputer result, expected a FinalOutput.");
                }

                signal = result.LastOutputValue.Value;
            }

            return signal;
        }
    }
}
