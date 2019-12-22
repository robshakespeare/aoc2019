using AoC.Day22;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day22
{
    public class CardShufflerTests
    {
        private static readonly CardShuffler Sut = new CardShuffler();

        [Test]
        public void Shuffle_TestCase1()
        {
            // ACT
            var result = Sut.Shuffle(@"deal with increment 7
deal into new stack
deal into new stack", 10);

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] { 0, 3, 6, 9, 2, 5, 8, 1, 4, 7 },
                options => options.WithStrictOrdering());
        }

        [Test]
        public void Shuffle_TestCase2()
        {
            // ACT
            var result = Sut.Shuffle(@"cut 6
deal with increment 7
deal into new stack", 10);

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] { 3, 0, 7, 4, 1, 8, 5, 2, 9, 6 },
                options => options.WithStrictOrdering());
        }

        [Test]
        public void Shuffle_TestCase3()
        {
            // ACT
            var result = Sut.Shuffle(@"deal with increment 7
deal with increment 9
cut -2", 10);

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] { 6, 3, 0, 7, 4, 1, 8, 5, 2, 9 },
                options => options.WithStrictOrdering());
        }

        [Test]
        public void Shuffle_TestCase4()
        {
            // ACT
            var result = Sut.Shuffle(@"deal into new stack
cut -2
deal with increment 7
cut 8
cut -4
deal with increment 7
cut 3
deal with increment 9
deal with increment 3
cut -1", 10);

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] { 9, 2, 5, 8, 1, 4, 7, 0, 3, 6 },
                options => options.WithStrictOrdering());
        }
    }
}
