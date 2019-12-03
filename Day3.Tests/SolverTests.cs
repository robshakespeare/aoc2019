using NUnit.Framework;

namespace Day3.Tests
{
    public class SolverTests
    {
        [TestCase(
            "R8,U5,L5,D3",
            "U7,R6,D4,L4",
            30)]
        [TestCase(
            "R75,D30,R83,U83,L12,D49,R71,U7,L72",
            "U62,R66,U55,R34,D71,R55,D58,R83",
            610)]
        [TestCase(
            "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
            "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7",
            410)]
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