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
            throw new NotImplementedException("rs-todo: finish tidy up!");
        }

        public long ShuffleThenGetCardAtIndex(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize,
            long numOfShuffles,
            long cardIndex)
        {
            throw new NotImplementedException("rs-todo: finish tidy up!");
        }

        private static (BigInteger increment_mul, BigInteger offset_diff) ComposeToSingleLinearPolynomial(
            (Technique technique, int operand)[] shuffleProcess,
            long deckSize)
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
                    // https://stackoverflow.com/a/15768873
                    // ModPow to emulate ModInverse, if `deckSize` is a prime
                    // i.e. modinv(x,n) == pow(x,n-2,n) for prime n
                    increment *= BigInteger.ModPow(operand, deckSize - 2, deckSize);
                }
                else
                {
                    throw new InvalidOperationException("Unexpected technique: " + technique);
                }

                increment = Modulus(increment, deckSize);
                offset = Modulus(offset, deckSize);
            }

            return (increment, offset);
        }

        /// <summary>
        /// Returns the modulus that results from division with two specified BigInteger values. https://stackoverflow.com/a/18106623
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        private static BigInteger Modulus(BigInteger dividend, BigInteger divisor) => (dividend % divisor + divisor) % divisor;
    }
}
