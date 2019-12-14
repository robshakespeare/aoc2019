using System.Linq;

namespace Day14
{
    public class Reaction
    {
        public ChemicalOutput Output { get; }
        public ChemicalInput[] Inputs { get; }

        public Reaction(ChemicalOutput output, params ChemicalInput[] inputs)
        {
            Output = output;
            Inputs = inputs;
        }

        ////public bool DependsOn(string chemical) => Inputs.Any(x => x.Chemical == chemical);

        public override string ToString() => $"{string.Join(", ", Inputs)} => {Output}";
    }
}
