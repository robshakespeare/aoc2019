using System.IO;

namespace Common
{
    public class InputLoaderReadAllLines : IInputLoader<string[]>
    {
        public string[] LoadInput()
        {
            using var _ = new TimingBlock("Load input (as lines)");
            return File.ReadAllLines("input.txt");
        }
    }
}
