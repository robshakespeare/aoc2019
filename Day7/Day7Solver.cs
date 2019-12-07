using System;
using Common;
using Common.IntCodes;

namespace Day7
{
    public class Day7Solver : SolverReadAllText
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();

        public override int? SolvePart1(string inputProgram)
        {
            return base.SolvePart1(inputProgram);
        }

        public int TryPhaseSettingSequence(string inputProgram, int[] phaseSettingSequence)
        {
            var signal = 0;

            foreach (var phaseSetting in phaseSettingSequence)
            {
                var result = intCodeComputer.ParseAndEvaluate(inputProgram, phaseSetting, signal);

                if (result.FinalOutput == null)
                {
                    throw new InvalidOperationException("Invalid IntCodeComputer result, expected a FinalOutput.");
                }

                signal += result.FinalOutput.Value;
            }

            return signal;
        }

        public override int? SolvePart2(string inputProgram)
        {
            return base.SolvePart2(inputProgram);
        }
    }
}
