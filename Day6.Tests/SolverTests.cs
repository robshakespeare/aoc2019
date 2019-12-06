using Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Day6.Tests
{
    public class SolverTests
    {
        private readonly Solver sut = new Solver();

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
