using AoC.Day22;
using Common;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day22
{
    public class CardShufflerV2Tests
    {
        private static readonly CardShufflerV2 Sut = new CardShufflerV2();

        private static readonly ShuffleProcessParser Parser = new ShuffleProcessParser();

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
