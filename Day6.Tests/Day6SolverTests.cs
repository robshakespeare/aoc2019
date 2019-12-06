using Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Day6.Tests
{
    public class Day6SolverTests
    {
        private readonly Day6Solver sut = new Day6Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(171213);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(292);
        }

        [Test]
        public void SpecExampleTest_Part1()
        {
            const string mapLines = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L";

            // ACT
            var result = sut.SolvePart1(mapLines.ReadAllLines());

            // ASSERT
            result.Should().Be(42);
        }

        [Test]
        public void SpecExampleTest_Part2()
        {
            const string mapLines = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
K)YOU
I)SAN";

            // ACT
            var result = sut.SolvePart2(mapLines.ReadAllLines());

            // ASSERT
            result.Should().Be(4);
        }
    }
}
