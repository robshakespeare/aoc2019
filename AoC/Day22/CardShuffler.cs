using System;
using System.Linq;

namespace AoC.Day22
{
    public class CardShuffler
    {
        private readonly int factoryOrderNumber;

        public CardShuffler(in int factoryOrderNumber)
        {
            this.factoryOrderNumber = factoryOrderNumber;
        }

        public int[] Shuffle((InstructionType instruction, int operand)[] shuffleProcess)
        {
            var factoryOrderDeck = Enumerable.Range(0, factoryOrderNumber).ToArray();
            var deck = factoryOrderDeck;

            return Shuffle(deck, shuffleProcess);
        }

        public int[] Shuffle(int[] deck, (InstructionType instruction, int operand)[] shuffleProcess)
        {
            foreach (var (instruction, operand) in shuffleProcess)
            {
                deck = instruction switch
                    {
                    InstructionType.DealIntoNewStack => DealIntoNewStack(deck),
                    InstructionType.Cut => Cut(deck, operand),
                    InstructionType.DealWithIncrement => DealWithIncrement(deck, operand),
                    _ => throw new InvalidOperationException("Unexpected instruction type: " + instruction)
                    };
            }

            return deck;
        }

        private static int[] DealIntoNewStack(in int[] deck) =>
            deck.Reverse().ToArray();

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
