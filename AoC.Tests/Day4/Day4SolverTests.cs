using AoC.Day4;
using NUnit.Framework;

namespace AoC.Tests.Day4
{
    public class Day4SolverTests
    {
        private readonly Day4Solver sut = new Day4Solver();

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var solution = sut.SolvePart2();

            // ASSERT
            Assert.AreEqual(1277, solution);
        }
    }
}
