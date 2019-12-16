using AoC.Day16;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day16
{
    public class PatternGeneratorTests
    {
        [Test]
        public void GeneratePatterns_ExampleCase1()
        {
            var sut = new PatternGenerator();

            // ACT
            var result = sut.GeneratePatterns(8);

            // ASSERT
            result.Should().BeEquivalentTo(
                new[]
                {
                    new[] { P(1, 0), P(-1, 2), P(1, 4), P(-1, 6) },
                    new[] { P(1, 1), P(1, 2), P(-1, 5), P(-1, 6) },
                    new[] { P(1, 2), P(1, 3), P(1, 4) },
                    new[] { P(1, 3), P(1, 4), P(1, 5), P(1, 6) },
                    new[] { P(1, 4), P(1, 5), P(1, 6), P(1, 7) },
                    new[] { P(1, 5), P(1, 6), P(1, 7) },
                    new[] { P(1, 6), P(1, 7) },
                    new[] { P(1, 7) }
                },
                options => options.WithStrictOrdering());
        }

        private static (int value, int index) P(int value, int index) => (value, index);
    }
}
