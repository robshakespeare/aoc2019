using System.IO;

namespace Common
{
    public class InputLoaderReadAllText : IInputLoader<string>
    {
        public string LoadInput()
        {
            using var _ = new TimingBlock("Load input (as text)");
            return File.ReadAllText("input.txt");
        }
    }
}
