using System;

namespace Common
{
    public static class MathUtils
    {
        /// <summary>
        /// Use Euclid's algorithm to calculate the greatest common divisor (GCD) of two numbers.
        /// From: http://csharphelper.com/blog/2014/08/calculate-the-greatest-common-divisor-gcd-and-least-common-multiple-lcm-of-two-integers-in-c/
        /// </summary>
        public static long GreatestCommonDivisor(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            // Pull out remainders.
            for (; ; )
            {
                long remainder = a % b;
                if (remainder == 0) return b;
                a = b;
                b = remainder;
            }
        }

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
