using System;
using System.IO;
using Serilog;
using Serilog.Core;

namespace AoC
{
    public static class Logging
    {
        private static string LogFolderPath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".aoc2019");

        private static readonly string LogFilePath = Path.Combine(LogFolderPath, "log.txt");

        public static Logger Logger { get; }

        static Logging()
        {
            Directory.CreateDirectory(LogFolderPath);

            Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(LogFilePath)
                .CreateLogger();
        }
    }
}
