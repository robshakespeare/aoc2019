using AoC.Day22;
using Common;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day22
{
    public class CardShufflerTests
    {
        private static readonly CardShuffler Sut = new CardShuffler();

        private static readonly ShuffleProcessParser Parser = new ShuffleProcessParser();

        private static int[] Shuffle(string shuffleProcessInstructions) => Sut.Shuffle(
            Parser.Parse(shuffleProcessInstructions),
            10,
            1);

        [Test]
        public void Shuffle_TestCase1()
        {
            // ACT
            var result = Shuffle(@"deal with increment 7
deal into new stack
deal into new stack");

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] { 0, 3, 6, 9, 2, 5, 8, 1, 4, 7 },
                options => options.WithStrictOrdering());
        }

        [Test]
        public void Shuffle_TestCase2()
        {
            // ACT
            var result = Shuffle(@"cut 6
deal with increment 7
deal into new stack");

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] { 3, 0, 7, 4, 1, 8, 5, 2, 9, 6 },
                options => options.WithStrictOrdering());
        }

        [Test]
        public void Shuffle_TestCase3()
        {
            // ACT
            var result = Shuffle(@"deal with increment 7
deal with increment 9
cut -2");

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] { 6, 3, 0, 7, 4, 1, 8, 5, 2, 9 },
                options => options.WithStrictOrdering());
        }

        [Test]
        public void Shuffle_TestCase4()
        {
            // ACT
            var result = Shuffle(@"deal into new stack
cut -2
deal with increment 7
cut 8
cut -4
deal with increment 7
cut 3
deal with increment 9
deal with increment 3
cut -1");

            // ASSERT
            result.Should().BeEquivalentTo(
                new[] { 9, 2, 5, 8, 1, 4, 7, 0, 3, 6 },
                options => options.WithStrictOrdering());
        }

        [Test]
        public void ShuffleThenGetIndexOfCard_Test()
        {
            var shuffleProcess = Parser.Parse(new InputLoaderReadAllText(22).LoadInput());
            const int cardNumber = 2019;

            // ACT
            var cardIndex = Sut.ShuffleThenGetIndexOfCard(shuffleProcess, 10007, 1, cardNumber);

            // ASSERT
            cardIndex.Should().Be(2306);
        }

        [Test]
        public void ShuffleThenGetCardAtIndex_Test()
        {
            var shuffleProcess = Parser.Parse(new InputLoaderReadAllText(22).LoadInput());
            const int cardIndex = 2306;

            // ACT
            var cardNumber = Sut.ShuffleThenGetCardAtIndex(shuffleProcess, 10007, 1, cardIndex);

            // ASSERT
            cardNumber.Should().Be(2019);
        }
    }
}
