using System.Linq;

namespace AoC.Day8
{
    public class Layer
    {
        public int[] Pixels { get; }

        public Layer(int[] pixels)
        {
            Pixels = pixels;
        }

        public int CountNumberOfDigits(int digit) => Pixels.Count(pixel => pixel == digit);
    }
}
