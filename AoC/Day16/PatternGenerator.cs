using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AoC.Day16
{
    public class PatternGenerator
    {
        private readonly int[] basePattern;

        public PatternGenerator()
            : this(0, 1, 0, -1)
        {
        }

        public PatternGenerator(params int[] basePattern)
        {
            this.basePattern = basePattern;
        }

        public IReadOnlyCollection<IReadOnlyCollection<(int value, int index)>> GeneratePatterns(int inputLength) =>
            Enumerable
                .Range(1, inputLength)
                .Select(elementNumber => GeneratePattern(elementNumber, inputLength))
                .ToReadOnlyArray();

        private IReadOnlyCollection<(int value, int index)> GeneratePattern(int elementNumber, int inputLength)
        {
            var expandedBasePattern = ExpandBasePattern(elementNumber);

            var requiredLengthIncludingSkip = (decimal)inputLength + 1;
            var timesToRepeat = (int) Math.Ceiling(requiredLengthIncludingSkip / expandedBasePattern.Count);
            return Enumerable
                .Repeat(expandedBasePattern, timesToRepeat)
                .SelectMany(x => x)
                .Skip(1)
                .Take(inputLength)
                .Select((value, index) => (value, index))
                .Where(x => x.value != 0)
                .ToReadOnlyArray();
        }

        private IReadOnlyCollection<int> ExpandBasePattern(int elementNumber) =>
            basePattern
                .SelectMany(x => Enumerable.Repeat(x, elementNumber))
                .ToReadOnlyArray();
    }
}
