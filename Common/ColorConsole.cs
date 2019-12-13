using System;

namespace Common
{
    public static class ColorConsole
    {
        public static void WriteLine(object? obj, ConsoleColor color)
        {
            var restoreColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(obj);
            }
            finally
            {
                Console.ForegroundColor = restoreColor;
            }
        }

        public static void Write(object? obj, ConsoleColor color)
        {
            var restoreColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.Write(obj);
            }
            finally
            {
                Console.ForegroundColor = restoreColor;
            }
        }
    }
}
