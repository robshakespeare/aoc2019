using System;
using System.Numerics;

namespace AoC.Day22
{
    /// <remarks>
    /// Totally struggled on part 2 of Day 22!
    /// I could see how it could be possible, but just couldn't do it without help!
    /// Useful resources were:
    ///  - https://www.reddit.com/r/adventofcode/comments/ee0rqi/2019_day_22_solutions/fbsm39r/
    ///  - https://www.reddit.com/r/adventofcode/comments/ee0rqi/2019_day_22_solutions/fbpz92k/
    ///  - https://www.reddit.com/r/adventofcode/comments/ee0rqi/2019_day_22_solutions/fbnkaju/
    ///  - https://www.reddit.com/r/adventofcode/comments/ee0rqi/2019_day_22_solutions/fbwauzi/
    ///  - https://github.com/kbmacneal/adv_of_code_2019/blob/3bdc583ea5620296e187a076038ddde17e526abd/days/22.cs
    ///  - https://topaz.github.io/paste/#XQAAAQDbCAAAAAAAAAARiEJHiiMzw3cPM/1Vl+2nx/DqKkM2yi+HVdpp+qLh9Jwh+ZECcFH/z2ezeBhLAAlZkhU0of3X0pFe6IC5Pt8zg+VNLW+7Hk/5EV5YpqOp6CSqtGyVPuSZaaoRLb7LS9oSqXoOJ/1joxUBAhkJcQvg07AIO5qtpviCy3hmvbmUKkGpJCyUBuCAJxm5+D5dys7Df5zKP6yNPHru9X91OmoufXhhi6kh2VywYD+wXQr44lxaXITD51KaaRa9VL3swZs0AP+RmWm3ptN717QGopPq2DzQJlu6F+YEfCHXb0J2mXftgPNdPnQhmV62/u+FcBFo+Fh4nNcmMwIQ97o736Ze/1kpuZoUoAW0pZtoITllJRYQ7nnF2vyqlblAT1eeg1CWtRQazBy+XZiPEmvvgYjYNgtI8C6pr/xfh/dIgWTJakJ7jxOKvehmqRLFofjxc+xxVURjkqOYAwMVaJRlN2hhu+d9LYWiJk5wqPW60coXFfhSU6QyRNqHWgSOkfisJ+qVUsI0z2eLeWkZtJHYELSUrRGHwXyBOIMqpTZVIEN5C4h+Du4upTtIzNe1OHn28CO6bgkC121maLN7idJD/rMtDilJjBmYI+/WITAyW+oBsN/3InB0fQSou+Tznl9e99m1hhDxLFSFAbPx5PSK3XBqIMpr1AIbeDZ0jMXFDE1qKjEJ9Ru0W1W5iK+Ffx5tcKTbpdAeqkHXruc0J6TGcwUdnoNPsz+QxLke1zt+O89z/PhnvIlkXS/tO17RJKCdL2Jf60Io+6d377cHi8znL19z7m1lTMvp67bP7nPHzQaw0zeAXbNpGdm6v+ZHIRJ0DR8WE7daQzcQKRFXbTnJe0yJji4hpf1EF3LxkTEjLeLuq2yxtw9e4wEguBgUVyK+YhwrJBoj2X+RGCh96cakHGuhY24z0xk0An1JtBNypGpnPntS4omSrBb8VsLHxziD/rNrxh34UUOI3cjek1z1Hu+seKrP3IJMdR6zQvistqcFD2/mJAMvQcpllWKwjrFdZ0Wap8w9LHFLMWUEPDOgv7nNB8OdvsFr7wO/5PWNlKgnReA/jLW/1iathfe5ADqzd5wmDFk6RTWMasO1+b4CFEEDM1sjq78KjxkF0hsgMS0wA/RuK5yGt14QIwqvFoTRVgNn3+/nCqWkGLnaziBLTF5F2fB0AZLmeL4vq5UybptsKVaxtpXOlROAjAgIs2GW6GnDVjlnwOR+vV5GeULzRvqSP30b1JShOLYGb+mp2Me0fdS6gQv90uqkA8sv1ZdQ/477AYAPBPDJ+10xIGcPzWItPpk0mhuf6ec0XhTp9wqrjjekK4sHYP64S1447X/nVWmDWFRx1//49ZZb
    ///  - https://github.com/metalim/metalim.adventofcode.2019.python/blob/master/22_cards_shuffle.ipynb
    ///  - https://stackoverflow.com/questions/7483706/c-sharp-modinverse-function
    /// </remarks>
    public class CardShufflerV2
    {
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
