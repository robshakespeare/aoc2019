using AoC.Day11;
using Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day11
{
    public class Day11SolverTests
    {
        private readonly Day11Solver sut = new Day11Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(2093);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            var expectedResult = @"Registration identifier:
 ███    ██ ███  █  █ █      ██ █  █ ███    
 █  █    █ █  █ █ █  █       █ █  █ █  █   
 ███     █ █  █ ██   █       █ █  █ █  █   
 █  █    █ ███  █ █  █       █ █  █ ███    
 █  █ █  █ █ █  █ █  █    █  █ █  █ █      
 ███   ██  █  █ █  █ ████  ██   ██  █      ".NormalizeLineEndings();

            part2Result.Should().Be(expectedResult);
        }
    }
}
