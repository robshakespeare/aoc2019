using System;
using Common;

namespace Day8
{
    public class Day8Solver : SolverReadAllText
    {
        private static Image CreateImage(string input)
        {
            const int imageWidth = 25;
            const int imageHeight = 6;

            var image = Image.Parse(input, imageWidth, imageHeight);
            return image;
        }

        public override int? SolvePart1(string input)
        {
            var image = CreateImage(input);
            return image.GetCorruptionCheckDigit();
        }

        public override int? SolvePart2(string input)
        {
            var image = CreateImage(input);

            // rs-todo: sort out needing to do this!!
            Console.WriteLine($"Decoded image is:{Environment.NewLine}{image.DecodeAndRenderImage()}");

            return null;
        }
    }
}
