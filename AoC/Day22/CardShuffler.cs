using System;
using System.Linq;

namespace AoC.Day22
{
    public class CardShuffler
    {
        public int[] Shuffle(in string completeShuffleProcessInput, in int factoryOrderNumber)
        {
            var factoryOrderDeck = Enumerable.Range(0, factoryOrderNumber).ToArray();

            var shuffleProcessParser = new ShuffleProcessParser();
            var deck = factoryOrderDeck;

            foreach (var (instruction, operand) in shuffleProcessParser.Parse(completeShuffleProcessInput))
            {
                deck = instruction switch
                    {
                    InstructionType.DealIntoNewStack => DealIntoNewStack(deck),
                    InstructionType.CutPositive => CutPositive(deck, operand),
                    InstructionType.CutNegative => CutNegative(deck, operand),
                    InstructionType.DealWithIncrement => DealWithIncrement(deck, operand),
                    _ => throw new InvalidOperationException("Unexpected instruction type: " + instruction)
                    };
            }

            return deck;
        }

        private static int[] DealIntoNewStack(in int[] deck) =>
            deck.Reverse().ToArray();

        private static int[] CutPositive(in int[] deck, in int numCards) =>
            deck.Skip(numCards).Concat(deck.Take(numCards)).ToArray();

        private static int[] CutNegative(in int[] deck, in int numCards)
        {
            var offset = deck.Length - numCards;
            return deck.Skip(offset).Concat(deck.Take(offset)).ToArray();
        }

        private static int[] DealWithIncrement(in int[] deck, in int increment)
        {
            var result = new int?[deck.Length];
            var index = 0;
            foreach (var card in deck)
            {
                if (result[index] != null)
                {
                    throw new InvalidOperationException(
                        $"Cannot deal card {card} with increment {increment}, index {index} has already been dealt to with card {result[index]}");
                }

                result[index] = card;
                index = (index + increment) % deck.Length;
            }

            return result
                .Select((card, resultIndex) =>
                    card
                    ?? throw new InvalidOperationException($"Invalid deal with increment, {resultIndex} was not dealt to."))
                .ToArray();
        }
    }
}
