using FluentAssertions;
using NUnit.Framework;

namespace Day8.Tests
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
            // rs-todo: should be the decoded string representation of the image
            part2Result.Should().Be(null);

            /*
             * Should be asserting that it equals:
Decoded image is:
 ██  ████ █  █ █  █  ██
█  █ █    █ █  █  █ █  █
█    ███  ██   █  █ █  █
█    █    █ █  █  █ ████
█  █ █    █ █  █  █ █  █
 ██  ████ █  █  ██  █  █
             */
        }
    }
}
