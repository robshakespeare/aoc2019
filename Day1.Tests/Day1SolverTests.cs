using FluentAssertions;
using NUnit.Framework;

namespace Day1.Tests
{
    public class ProgramTests
    {
        private readonly Program sut = new Program();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(3465245);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(5194970);
        }
    }
}
