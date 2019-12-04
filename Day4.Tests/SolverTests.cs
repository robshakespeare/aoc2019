using NUnit.Framework;

namespace Day4.Tests
{
    public class SolverTests
    {
        private readonly Solver _sut = new Solver();

        [Test]
        public void HowManyDifferentPasswordsWithinRange_Test()
        {
            // ACT
            var solution = _sut.HowManyDifferentPasswordsWithinRange(Day4.Program.Input);

            // ASSERT
            Assert.AreEqual(1277, solution);
        }
    }
}
