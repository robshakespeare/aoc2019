using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Extensions;
using Common.IntCodes;

namespace AoC.Day25
{
    public class AsciiComputer
    {
        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();
        private readonly Queue<long> inputs = new Queue<long>();

        private readonly StringBuilder outputBuffer = new StringBuilder();

        /// <summary>
        /// Parses and then evaluates the specified ASCII IntCode computer until it halts, using input and output redirection.
        /// </summary>
        public IntCodeState ParseAndEvaluate(string inputProgram, Func<string> provideInput, Action<string> consumeOutput) =>
            intCodeComputer.ParseAndEvaluate(
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
                    Console.Write((char) input);
                    return input;
                },
                output =>
                {
                    if (output == '\n')
                    {
                        consumeOutput(outputBuffer.ToString());
                        outputBuffer.Clear();
                    }
                    else
                    {
                        outputBuffer.Append(Decode(output));
                    }
                });

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
