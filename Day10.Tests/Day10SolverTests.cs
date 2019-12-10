using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace Day10.Tests
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

        //[Test]
        //public void CalculateAngleBetweenVector_Tests()
        //{
        //    // ACT
        //    var result1 = sut.CalculateAngleBetweenVectors(new PointF(1, 1), new PointF(1, 1));
        //    var result2 = sut.CalculateAngleBetweenVectors(new PointF(1, 1), new PointF(1, 2));
        //    var result3 = sut.CalculateAngleBetweenVectors(new PointF(1, 1), new PointF(1, 3));

        //    // ASSERT
        //    result1.Should().Be(result2);
        //    result1.Should().Be(result3);
        //}

        //[Test]
        //public void CalculateAngleBetweenVector_Tests_2()
        //{
        //    // ACT
        //    var result1 = sut.CalculateAngleBetweenVectors(new PointF(2.1f, 2.1f), new PointF(2.2f, 2.2f));
        //    var result2 = sut.CalculateAngleBetweenVectors(new PointF(3.1f, 3.1f), new PointF(4.1f, 4.1f));

        //    // ASSERT
        //    result1.Should().Be(result2);
        //}
    }
}
