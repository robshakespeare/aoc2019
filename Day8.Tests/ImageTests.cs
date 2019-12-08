using FluentAssertions;
using NUnit.Framework;

namespace Day8.Tests
{
    public class ImageTests
    {
        [Test]
        public void Parse_Test()
        {
            // ACT
            var result = Image.Parse("123456789012", 3, 2);

            // ASSERT
            result.Should().BeEquivalentTo(
                new
                {
                    Layers = new[]
                    {
                        new {Pixels = new[] {1, 2, 3, 4, 5, 6}},
                        new {Pixels = new[] {7, 8, 9, 0, 1, 2}}
                    },
                    ImageSize =new
                    {
                        Width = 3,
                        Height = 2
                    },
                    LayerLength = 6
                });
        }

        [Test]
        public void GetCorruptionCheckDigit_Test()
        {
            var sut = Image.Parse("121216789012", 3, 2);

            // ACT
            var result = sut.GetCorruptionCheckDigit();

            // ASSERT
            result.Should().Be(3 * 2);
        }
    }
}
