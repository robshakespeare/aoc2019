using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Extensions;

namespace Day14
{
    public class NanoFactory
    {
        private const string ORE = "ORE";
        private const string FUEL = "FUEL";

        /// <summary>
        /// Dictionary of all the reactions, where KEY is Output Chemical, VALUE is the full reaction for that Output Chemical.
        /// </summary>
        private readonly Dictionary<string, Reaction> reactions;

        /// <summary>
        /// Constructor
        /// </summary>
        public NanoFactory(Dictionary<string, Reaction> reactions)
        {
            this.reactions = reactions;
        }

        /// <summary>
        /// Calculates the amount of ORE required to reach the specified amount of fuel.
        /// </summary>
        public long CalculateOreRequired(int fuelQuantityRequired)
        {
            var store = new ChemicalQuantityMap();
            var totalProduce = new ChemicalQuantityMap();
            var oreList = new List<int>();

            Produce(
                FUEL,
                fuelQuantityRequired,
                store,
                totalProduce,
                oreList);

            var oreRequired = oreList.Sum();
            return oreRequired;
        }

        private void Produce(
            in string chemicalRequired,
            int amountRequired,
            in ChemicalQuantityMap store,
            in ChemicalQuantityMap totalProduce,
            in List<int> oreList)
        {
            // First, use any quantity we have remaining for this chemical
            store.DepleteIfAvailable(chemicalRequired, ref amountRequired);

            // Only produce more if we need it, otherwise, just exit this method now.
            if (amountRequired == 0)
            {
                return;
            }

            // Get the reaction that will produce the chemical required
            var reaction = reactions[chemicalRequired];

            // Run the reaction
            var numRunsRequired = (int) Math.Ceiling(amountRequired / (decimal) reaction.Output.Quantity);

            foreach (var input in reaction.Inputs)
            {
                if (input.Chemical == ORE)
                {
                    oreList.Add(input.Quantity * numRunsRequired);
                }
                else
                {
                    Produce(input.Chemical, input.Quantity * numRunsRequired, store, totalProduce, oreList);
                }
            }

            // Reaction has now produced, so perform updates
            var amountProduced = reaction.Output.Quantity * numRunsRequired;

            totalProduce.IncreaseQuantity(chemicalRequired, amountProduced);
            store.IncreaseQuantity(chemicalRequired, amountProduced - amountRequired);
        }

        private static readonly Regex LineRegex = new Regex(@"(?<inputs>.+) => (?<quanity>\d+) (?<chemical>\w+)", RegexOptions.Compiled);

        private static readonly Regex InputRegex = new Regex(@"(?<quanity>\d+) (?<chemical>[^ ,]+)", RegexOptions.Compiled);

        public static Dictionary<string, Reaction> ParseReactions(string input)
        {
            var result = new Dictionary<string, Reaction>();
            var matches = LineRegex.Matches(input.NormalizeLineEndings()).Where(x => x != null).ToArray();

            if (matches.Length == 0)
            {
                throw new InvalidOperationException("Invalid input, not a valid list of reactions");
            }

            foreach (var match in matches)
            {
                var output = new ChemicalQuantity(int.Parse(match.Groups["quanity"].Value), match.Groups["chemical"].Value);

                var inputs = InputRegex.Matches(match.Groups["inputs"].Value).Where(x => x != null).ToArray();
                if (inputs.Length == 0)
                {
                    throw new InvalidOperationException("Invalid input, not a valid reaction: " + match.Value);
                }

                result.Add(
                    output.Chemical,
                    new Reaction(
                        output,
                        inputs
                            .Select(inMatch => new ChemicalQuantity(int.Parse(inMatch.Groups["quanity"].Value), inMatch.Groups["chemical"].Value))
                            .ToArray()));
            }

            return result;
        }

        public static NanoFactory Create(string input) => new NanoFactory(ParseReactions(input));
    }
}
