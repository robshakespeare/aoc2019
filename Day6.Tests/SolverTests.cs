using System;
using Common.Streams;
using FluentAssertions;
using NUnit.Framework;

namespace Day6.Tests
{
    public class SolverTests
    {
        private readonly Solver sut = new Solver();

        [Test]
        public void SpecExampleTest()
        {
            // rs-todo: fix issue with new line conflicts here
            var mapLines = @"COM)B
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
            var result = sut.WhatIsTheTotalNumberOfDirectAndIndirectOrbits(StreamUtils.ToStream(mapLines));

            // ASSERT
            result.Should().Be(42);
        }

        [Test]
        public void SpecExampleTest_Part2()
        {
            // rs-todo: fix issue with new line conflicts here
            var mapLines = @"COM)B
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
            var result = sut.SolvePart2(mapLines.Split(Environment.NewLine));

            // ASSERT
            result.Should().Be(4);
        }
    }
}
