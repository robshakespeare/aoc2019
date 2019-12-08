using System;
using Common;

namespace Day8
{
    public class Day8Solver : Solver<Image>
    {
        public Day8Solver() : base(new InputLoaderDelegated<Image>(LoadInputImage))
        {
        }

        private static Image LoadInputImage()
        {
            const int imageWidth = 25;
            const int imageHeight = 6;

            var input = new InputLoaderReadAllText().LoadInput();

            return Image.Parse(input, imageWidth, imageHeight);
        }

        public override int? SolvePart1(Image image) => image.GetCorruptionCheckDigit();

        public override int? SolvePart2(Image image)
        {
            // rs-todo: sort out needing to do this!!
            Console.WriteLine($"Decoded image is:{Environment.NewLine}{image.DecodeAndRenderImage()}");

            return null;
        }
    }
}