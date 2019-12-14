using System;
using System.Linq;
using AoC.Day10;
using Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day10
{
    public class Day10SolverTests
    {
        private readonly Day10Solver sut = new Day10Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(256);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(1707);
        }

        [Test]
        public void Part1_TestCase1()
        {
            const string input = @".#..#
.....
#####
....#
...##";

            // ACT
            var result = sut.SolvePart1(input.ReadAllLines());

            // ASSERT
            result.Should().Be(8);
        }

        [Test]
        public void GetNormalBetweenVectors_Test1()
        {
            var vectors = new[]
            {
                sut.GetNormalBetweenVectors(new Vector(3, 3), new Vector(0, 0)),
                sut.GetNormalBetweenVectors(new Vector(3.5, 3.5), new Vector(0, 0)),
                sut.GetNormalBetweenVectors(new Vector(4, 4), new Vector(0, 0)),
                sut.GetNormalBetweenVectors(new Vector(10, 10), new Vector(0, 0))
            };

            foreach (var vector in vectors)
            {
                Console.WriteLine(vector);
            }

            vectors.Distinct().Count().Should().Be(1);
        }
    }
}
