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
        [TestCase(111111, true)]
        [TestCase(999999, true)]
        [TestCase(999999 + 1, false)]
        [TestCase(1999999, false)]
        [TestCase(9999999, false)]
        [TestCase(122345, true)]
        [TestCase(111123, true)]
        [TestCase(133679, true)]
        [TestCase(135679, false)]
        [TestCase(223450, false)]
        [TestCase(123789, false)]
        [TestCase(135799, true)]
        [TestCase(112266, true)]
        public void IsValid_Tests(int inputNumber, bool expectedResult)
        {
            // ACT
            var actualResult = _sut.IsValid(inputNumber);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
