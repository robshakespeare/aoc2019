using Common.Extensions;

namespace Day12.Tests
{
    public static class StringExtensions
    {
        public static string NormalizePositionAndVelocityText(this string text) => text.NormalizeLineEndings().Replace("=  ", "=").Replace("= ", "=");
    }
}
