using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Streams
{
    public static class StreamUtils
    {
        public static IEnumerable<string> FileToEnumerableOfString(string filePath) => StreamToEnumerableOfString(File.OpenRead(filePath));

        public static IEnumerable<string> StreamToEnumerableOfString(Stream stream)
        {
            using var _ = stream;
            using var streamReader = new StreamReader(stream);

            while (!streamReader.EndOfStream)
            {
                yield return streamReader.ReadLine();
            }
        }

        // rs-todo: might as well go straight to enum of lines, and then rename this class so its obvious we just do "to lines"
        public static Stream ToStream(string s) => new MemoryStream(Encoding.UTF8.GetBytes(s));
    }
}
