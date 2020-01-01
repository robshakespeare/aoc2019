using System;
using System.Numerics;

namespace AoC.Day22
{
    public class CardShufflerV2 : ICardShuffler
    {
        public long ShuffleThenGetIndexOfCard(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize,
            long cardNumber)
        {
            var (increment, offset) = ComposeToSingleLinearPolynomial(shuffleProcess, deckSize, 1);
            Console.WriteLine(new { increment, offset });
            return (long) ((cardNumber - offset) / increment);
            //return (long) (offset + increment * cardNumber) % deckSize;

            // number = offset + increment * index

            // cardNumber = (offset + index * increment) % deckSize
        }

        public long ShuffleThenGetCardAtIndex(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize,
            long numOfShuffles,
            long cardIndex)
        {
            var (increment, offset) = ComposeToSingleLinearPolynomial(shuffleProcess, deckSize, numOfShuffles);
            return (long) ((offset + cardIndex * increment) % deckSize);
        }

        private static (BigInteger increment_mul, BigInteger offset_diff) ComposeToSingleLinearPolynomial(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize,
            long numOfShuffles)
        {
            var increment = new BigInteger(1);
            var offset = new BigInteger(0);

            foreach (var (technique, operand) in shuffleProcess)
            {
                if (technique == Technique.DealIntoNewStack)
                {
                    increment *= -1;
                    offset += increment;
                }
                else if (technique == Technique.Cut)
                {
                    offset += operand * increment;
                }
                else if (technique == Technique.DealWithIncrement)
                {
                    increment *= ModInverse(operand, deckSize);
                }
                else
                {
                    throw new InvalidOperationException("Unexpected technique: " + technique);
                }

                increment = Mod(increment, deckSize);
                offset = Mod(offset, deckSize);
            }

            var incrementExponentiated = BigInteger.ModPow(increment, numOfShuffles, deckSize);
            offset = offset * (1 - incrementExponentiated) * ModInverse((1 - increment) % deckSize, deckSize);
            offset %= deckSize;

            return (incrementExponentiated, offset);
        }

        /// <summary>
        /// Emulate ModInverse, using ModPow, if n is a prime. https://stackoverflow.com/a/15768873
        /// i.e. modinv(x,n) == pow(x,n-2,n) for prime n
        /// </summary>
        private static BigInteger ModInverse(BigInteger x, BigInteger n) => BigInteger.ModPow(x, n - 2, n);

        /// <summary>
        /// Returns the modulus that results from division with two specified BigInteger values. https://stackoverflow.com/a/18106623
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        private static BigInteger Mod(BigInteger dividend, BigInteger divisor) => (dividend % divisor + divisor) % divisor;
    }
}
