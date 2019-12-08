using System;
using Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Common.Tests.Extensions
{
    public class StringExtensionsTests
    {
        // Line Feed = \n
        // Carriage Return = \r

        [Test]
        public void NormalizeLineEndings_DoesNormalize_LineFeed()
        {
            // ACT
            var result = "test\nvalue\nhere".NormalizeLineEndings();

            // ASSERT
            result.Split(Environment.NewLine).Should().BeEquivalentTo(
                "test",
                "value",
                "here");
        }

        [Test]
        public void NormalizeLineEndings_DoesNormalize_CarriageReturn()
        {
            // ACT
            var result = "test\rvalue\rhere".NormalizeLineEndings();

            // ASSERT
            result.Split(Environment.NewLine).Should().BeEquivalentTo(
                "test",
                "value",
                "here");
        }

        [Test]
        public void NormalizeLineEndings_DoesNormalize_CarriageReturnLineFeed()
        {
            // ACT
            var result = "test\r\nvalue\r\nhere".NormalizeLineEndings();

            // ASSERT
            result.Split(Environment.NewLine).Should().BeEquivalentTo(
                "test",
                "value",
                "here");
        }

        [Test]
        public void NormalizeLineEndings_DoesNormalize_LineFeedCarriageReturn()
        {
            // ACT
            var result = "test\n\rvalue\n\rhere".NormalizeLineEndings();

            // ASSERT
            result.Split(Environment.NewLine).Should().BeEquivalentTo(
                "test",
                "value",
                "here");
        }

        [Test]
        public void NormalizeLineEndings_DoesNormalize_Mixture()
        {
            // ACT
            var result = "test\r\n\nvalue\r\r\n\nhere".NormalizeLineEndings();

            // ASSERT
            result.Split(Environment.NewLine).Should().BeEquivalentTo(
                "test",
                "",
                "value",
                "",
                "",
                "here");
        }
    }
}
