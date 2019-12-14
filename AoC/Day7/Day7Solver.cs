using System.Linq;
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
                .Select(phaseSettingSequence => intCodeComputer.ParseAndEvaluateWithPhaseSettingSequenceAndFeedbackLoop(inputProgram, phaseSettingSequence))
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
    }
}