using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Extensions;

namespace Common.IntCodes
{
    public class AsciiComputer
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();
        private readonly Queue<long> inputs = new Queue<long>();

        private readonly StringBuilder outputBuffer = new StringBuilder();

        /// <summary>
        /// Parses and then evaluates the specified ASCII IntCode computer until it halts, using input and output redirection.
        /// </summary>
        public IntCodeState ParseAndEvaluate(string inputProgram, Func<string> provideInput, Action<string> consumeOutput)
        {
            var intCodeState = intCodeComputer.ParseAndEvaluate(
                inputProgram,
                () =>
                {
                    if (inputs.Count == 0)
                    {
                        var inputText = provideInput();
                        Encode(inputText).ToList()
                            .ForEach(inputs.Enqueue);
                    }

                    var input = inputs.Dequeue();
                    ColorConsole.Write((char) input, ConsoleColor.Cyan);
                    return input;
                },
                output =>
                {
                    if (output == '\n')
                    {
                        FlushBuffer(consumeOutput);
                    }
                    else
                    {
                        outputBuffer.Append(Decode(output));
                    }
                });

            if (outputBuffer.Length > 0)
            {
                FlushBuffer(consumeOutput);
            }

            return intCodeState;
        }

        private void FlushBuffer(Action<string> consumeOutput)
        {
            var output = outputBuffer.ToString();
            ColorConsole.WriteLine(output, ConsoleColor.DarkGray);
            consumeOutput(output);
            outputBuffer.Clear();
        }

        private static IEnumerable<long> Encode(string s) =>
            s.NormalizeLineEndings()
                .ReadAllLines()
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .SelectMany(
                    line => line.ToCharArray()
                        .Select(c => (long) c)
                        .Append('\n'));

        private static string Decode(long outputValue) =>
            outputValue < 128 ? Encoding.ASCII.GetString(new[] {(byte) outputValue}) : "";
    }
}
