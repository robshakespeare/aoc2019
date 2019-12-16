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
                    new[] { 1, 0, -1, 0, 1, 0, -1, 0 },
                    new[] { 0, 1, 1, 0, 0, -1, -1, 0 },
                    new[] { 0, 0, 1, 1, 1, 0, 0, 0 },
                    new[] { 0, 0, 0, 1, 1, 1, 1, 0 },
                    new[] { 0, 0, 0, 0, 1, 1, 1, 1 },
                    new[] { 0, 0, 0, 0, 0, 1, 1, 1 },
                    new[] { 0, 0, 0, 0, 0, 0, 1, 1 },
                    new[] { 0, 0, 0, 0, 0, 0, 0, 1 },
                },
                options => options.WithStrictOrdering());
        }
    }
}
