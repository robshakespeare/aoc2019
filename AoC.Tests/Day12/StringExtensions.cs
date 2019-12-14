using Common.Extensions;

namespace AoC.Tests.Day12
{
    public static class StringExtensions
    {
        public static string NormalizePositionAndVelocityText(this string text) => text.NormalizeLineEndings().Replace("=  ", "=").Replace("= ", "=");
    }
}
