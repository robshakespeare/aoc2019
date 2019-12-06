using FluentAssertions;
using NUnit.Framework;

namespace Day6.Tests
{
    public class ProgramTests
    {
        private readonly Solver solver = new Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = Program.SolvePart1(solver);

            // ASSERT
            part1Result.Should().Be(171213);
        }
    }
}
