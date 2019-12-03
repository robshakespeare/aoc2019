using NUnit.Framework;

namespace Day3.Tests
{
    public class SolverTests
    {
        [TestCase(
            "R8,U5,L5,D3",
            "U7,R6,D4,L4",
            6)]
        public void Solve_Tests(string wire1, string wire2, int expectedResult)
        {
            var sut = new Solver();
            var wires = new[] {wire1, wire2};

            // ACT
            var result = sut.Solve(wires);

            // ASSERT
            Assert.AreEqual(expectedResult, result);
        }
    }
}