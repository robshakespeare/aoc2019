using AoC.Day18;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day18
{
    public class Day18SolverTests
    {
        private readonly Day18Solver sut = new Day18Solver();

        [Test]
        public void Part1_TestCase1()
        {
            // ACT
            var result = sut.SolvePart1(@"#########
#b.A.@.a#
#########");

            // ASSERT
            result.Should().Be(8);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(null);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(null);
        }
    }
}
