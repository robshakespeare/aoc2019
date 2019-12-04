using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Day4.Tests
{
    public class PasswordCriteriaValidatorTests
    {
        private PasswordCriteriaValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new PasswordCriteriaValidator();
        }

        [TestCase(0, false)]
        [TestCase(1000, false)]
        [TestCase(111111 - 1, false)]
        [TestCase(111111, false)]
        [TestCase(999999, false)]
        [TestCase(999999 + 1, false)]
        [TestCase(1999999, false)]
        [TestCase(9999999, false)]
        [TestCase(122345, true)]
        [TestCase(111123, false)]
        [TestCase(133679, true)]
        [TestCase(135679, false)]
        [TestCase(223450, false)]
        [TestCase(123789, false)]
        [TestCase(135799, true)]
        [TestCase(112266, true)]

        [TestCase(112233, true)]
        [TestCase(123444, false)]
        [TestCase(111122, true)]

        [TestCase(112333, true)]
        [TestCase(111333, false)]

        [TestCase(101234, false)]
        [TestCase(121234, false)]

        [TestCase(123455, true)]
        public void IsValid_Tests(int inputNumber, bool expectedResult)
        {
            // ACT
            var actualResult = _sut.IsValid(inputNumber);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCaseSource(nameof(GetAdjacentGroupLengths_TestCases))]
        public void GetAdjacentGroupLengths_Tests(string inputString, int[] expectedAdjacentGroupLengths)
        {
            // ACT
            var result = PasswordCriteriaValidator.GetAdjacentGroupLengths(inputString);

            // ASSERT
            result.Should().BeEquivalentTo(
                expectedAdjacentGroupLengths,
                options => options.WithStrictOrdering());
        }

        public static IEnumerable<TestCaseData> GetAdjacentGroupLengths_TestCases() => new[]
        {
            new TestCaseData("", new int[0]),
            new TestCaseData("112233", new[] { 2, 2, 2 }),
            new TestCaseData("123456", new[] { 1, 1, 1, 1, 1, 1 }),
            new TestCaseData("111111", new[] { 6 }),
            new TestCaseData("1", new[] { 1 }),
            new TestCaseData("111116", new[] { 5, 1 }),
            new TestCaseData("211111", new[] { 1, 5 }),
            new TestCaseData("221111", new[] { 2, 4 }),
            new TestCaseData("222111", new[] { 3, 3 }),
            new TestCaseData("111122", new[] { 4, 2 }),
            new TestCaseData("211222", new[] { 1, 2, 3 }),
            new TestCaseData("222112", new[] { 3, 2, 1 })
        };
    }
}
