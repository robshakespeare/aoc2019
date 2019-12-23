using System.Numerics;

namespace Common
{
    public static class MathUtils
    {
        /// <summary>
        /// Finds the greatest common divisor (GCD) of the two specified numbers.
        /// </summary>
        public static long GreatestCommonDivisor(long a, long b) => (long)BigInteger.GreatestCommonDivisor(a, b);

        /// <summary>
        /// Returns the least common multiple (LCM) of the two specified numbers.
        /// </summary>
        public static long LeastCommonMultiple(long a, long b) => a * b / GreatestCommonDivisor(a, b);

        /// <summary>
        /// Returns the least common multiple (LCM) of the three specified numbers.
        /// </summary>
        public static long LeastCommonMultiple(long a, long b, long c) => LeastCommonMultiple(LeastCommonMultiple(a, b), c);
    }
}
