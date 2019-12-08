using System;
using Common;

namespace Day8
{
    public class Day8Solver : Solver<Image, int?, string?>
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

        public override string? SolvePart2(Image image) => $"Decoded image is:{Environment.NewLine}{image.DecodeAndRenderImage()}";
    }
}
