using System;
using System.Linq;

namespace AoC.Day22
{
    public class CardShuffler
    {
        public long ShuffleThenGetIndexOfCard(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize,
            long cardNumber) =>
            Shuffle(shuffleProcess, deckSize, 1).ToList().IndexOf((int)cardNumber);

        public long ShuffleThenGetCardAtIndex(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize,
            long numOfShuffles,
            long cardIndex) =>
            Shuffle(shuffleProcess, deckSize, numOfShuffles)[cardIndex];

        public int[] Shuffle(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize,
            long numOfShuffles)
        {
            var deck = CreateFactoryOrderDeck(deckSize);

            for (var i = 0; i < numOfShuffles; i++)
            {
                deck = Shuffle(deck, shuffleProcess);
            }

            return deck;
        }

        private static int[] CreateFactoryOrderDeck(long deckSize)
        {
            if (deckSize > Int32.MaxValue)
            {
                throw new NotSupportedException("V1 shuffler only supports Int32 deck sizes");
            }

            return Enumerable.Range(0, (int)deckSize).ToArray();
        }

        private static int[] Shuffle(int[] deck, (Technique technique, int operand)[] shuffleProcess)
        {
            foreach (var (technique, operand) in shuffleProcess)
            {
                deck = technique switch
                {
                    Technique.DealIntoNewStack => DealIntoNewStack(deck),
                    Technique.Cut => Cut(deck, operand),
                    Technique.DealWithIncrement => DealWithIncrement(deck, operand),
                    _ => throw new InvalidOperationException("Unexpected technique: " + technique)
                };
            }

            return deck;
        }

        private static int[] DealIntoNewStack(in int[] deck) => deck.Reverse().ToArray();

        private static int[] Cut(in int[] deck, in int numCards)
        {
            var skipTake = numCards >= 0
                ? numCards
                : deck.Length + numCards;
            return deck.Skip(skipTake).Concat(deck.Take(skipTake)).ToArray();
        }

        private static int[] DealWithIncrement(in int[] deck, in int increment)
        {
            var result = new int[deck.Length];
            var index = 0;

            foreach (var card in deck)
            {
                result[index] = card;
                index = (index + increment) % deck.Length;
            }

            return result.ToArray();
        }
    }
}
