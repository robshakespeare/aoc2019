using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;

namespace Day8
{
    public class Image
    {
        public const int BlackPixel = 0;
        public const int WhitePixel = 1;
        public const int TransparentPixel = 2;

        public Layer[] Layers { get; }
        public Size ImageSize { get; }
        public int LayerLength { get; }

        public Image(Layer[] layers, Size imageSize, int layerLength)
        {
            Layers = layers;
            ImageSize = imageSize;
            LayerLength = layerLength;
        }

        public int GetCorruptionCheckDigit()
        {
            // To make sure the image wasn't corrupted during transmission, the Elves
            // would like you to find the layer that contains the fewest 0 digits. On that
            // layer, what is the number of 1 digits multiplied by the number of 2 digits?

            var layerWithFewest0Digits = Layers
                .OrderBy(layer => layer.CountNumberOfDigits(0))
                .First();

            return layerWithFewest0Digits.CountNumberOfDigits(1) * layerWithFewest0Digits.CountNumberOfDigits(2);
        }

        public static Image Parse(string input, int imageWidth, int imageHeight)
        {
            input = input.Trim();
            var layerLength = imageWidth * imageHeight;

            if (input.Length % layerLength != 0)
            {
                throw new InvalidOperationException("Invalid input, not exactly divisible by layer size.");
            }

            var allPixels = input.Select(c => int.Parse(new string(c, 1)));

            return new Image(
                allPixels
                    .Batch(layerLength)
                    .Select(layerPixels => new Layer(layerPixels.ToArray()))
                    .ToArray(),
                new Size(imageWidth, imageHeight), 
                layerLength);
        }

        public IEnumerable<IEnumerable<int>> DecodeImage()
        {
            var resultBuffer = new int[LayerLength];

            for (var pixelIndex = 0; pixelIndex < LayerLength; pixelIndex++)
            {
                var pixel = GetFirstNonTransparentPixelLayer(pixelIndex);
                resultBuffer[pixelIndex] = pixel;
            }

            return resultBuffer.Batch(ImageSize.Width);
        }

        public string DecodeAndRenderImage() =>
            string.Join(
                Environment.NewLine,
                DecodeImage()
                    .Select(line => line.Select(pixel => pixel == WhitePixel ? '█' : ' '))
                    .Select(line => string.Join("", line)));

        public IEnumerable<int> GetPixelLayers(int pixelIndex) => Layers.Select(layer => layer.Pixels[pixelIndex]);

        public int GetFirstNonTransparentPixelLayer(int pixelIndex) =>
            GetPixelLayers(pixelIndex)
                .SkipWhile(pixel => pixel == TransparentPixel)
                .First();
    }
}
