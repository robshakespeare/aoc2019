using System.IO;

namespace Common
{
    public abstract class SolverReadAllLines : SolverWithInput<string[]>
    {
        protected override string[] LoadInput()
        {
            using var _ = new TimingBlock("Load input");
            return File.ReadAllLines("input.txt");
        }
    }
}
