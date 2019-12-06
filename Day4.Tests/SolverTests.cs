using NUnit.Framework;

namespace Day4.Tests
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
