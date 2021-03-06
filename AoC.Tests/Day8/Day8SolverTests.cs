using AoC.Day8;
using Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day8
{
    public class Day8SolverTests
    {
        private readonly Day8Solver sut = new Day8Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(1548);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            var expectedResult = @"Decoded image is:
 ██  ████ █  █ █  █  ██  
█  █ █    █ █  █  █ █  █ 
█    ███  ██   █  █ █  █ 
█    █    █ █  █  █ ████ 
█  █ █    █ █  █  █ █  █ 
 ██  ████ █  █  ██  █  █ ".NormalizeLineEndings();

            part2Result.Should().Be(expectedResult);
        }
    }
}
