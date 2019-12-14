using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Extensions;

namespace Day14
{
    public class NanoFactory
    {
        public const string ORE = "ORE";
        public const string FUEL = "FUEL";

        // KEY is Output Chemical, VALUE is the full reaction for that Output Chemical
        private readonly Dictionary<string, Reaction> reactions; 

        

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
            Produce(new ChemicalOutput(fuelQuantityRequired, FUEL), fuelQuantityRequired, store, totalProduce);

            return totalProduce[ORE];

            // /*
            // * First, pass through, calculating the number of each chemical required
            // * Then, then, work out how many times that chemical's reaction would have to be done to achieve that quantity (i.e. ceiling(reactionOutQuantity / numRequired))
            // *  ^ this is recurssie tho, keeping track of all the totals
            // * Once done, we should have the total number of owe
            // *
            // */

            //var chemicalTotalsRequired = new Dictionary<string, int>();
            //EnumerateChemicalTotalsRequired(FUEL, fuelQuantityRequired, chemicalTotalsRequired);

            //return reactions
            //    .Where(reaction => reaction.Value.DependsOn(ORE))
            //    .Sum(reactionPair =>
            //    {
            //        var (reactionOutputChemical, reaction) = reactionPair;
            //        if (chemicalTotalsRequired.TryGetValue(reactionOutputChemical, out var chemicalTotalRequired))
            //        {
            //            var numReactionsRequired = (long)Math.Ceiling(chemicalTotalRequired / (decimal) reaction.Output.Quantity);
            //            return numReactionsRequired * reaction.Inputs.First(x => x.Chemical == ORE).Quantity;
            //        }

            //        return 0;
            //    });

            ////Produce(FUEL, fuelQuantityRequired);
            ////return chemicalQuantitiesProduced[ORE];
        }

        private void Produce(in ChemicalOutput chemicalRequired, int amountOfChemicalRequired, /*, int amountRequired*//*New, in decimal factor*/ in ChemicalQuantityMap store, in ChemicalQuantityMap totalProduce)
        {
            ////var amountRequired = amountRequiredNew * factor;

            var reaction = chemicalRequired.Chemical == ORE
                ? new Reaction(chemicalRequired)
                : reactions[chemicalRequired.Chemical];
            //// rs-todo: KEEP, just don't do runs for now! var numRunsRequired = (int)Math.Ceiling(amountRequired / (decimal) reaction.Output.Quantity);

            var factor = 1m;
            if (reaction.Output.Quantity != amountOfChemicalRequired)
            {
                factor = amountOfChemicalRequired / (decimal)reaction.Output.Quantity;
            }

            foreach (var input in reaction.Inputs)
            {
                // get how much is produced by the reaction that makes the input chemical
                //var reactionTotal = reactions[input.Chemical].Output.Quantity;

                //var newFactor = input.Quantity / (decimal)reactionTotal;

                //var newFactor = amountRequired / (decimal) input.Quantity;

                Consume(input, factor /*, numRunsRequired, newFactor*/ /*amountRequired * numRunsRequired,*/ /*input.Quantity * numRunsRequired*/, store, totalProduce);
            }

            // Add the amount of chemical produced to the "store" and to the list of totals produced
            var amountProduced = reaction.Output.Quantity; // * numRunsRequired;

            store.IncreaseQuantity(reaction.Output.Chemical, amountProduced);
            totalProduce.IncreaseQuantity(reaction.Output.Chemical, amountProduced);
        }

        private void Consume(in ChemicalInput input, decimal factor, /*, int numRunsRequired, in decimal factor*/ in ChemicalQuantityMap store, in ChemicalQuantityMap totalProduce)
        {
            //var amountToConsume = numRunsRequired * (int)Math.Round(input.Quantity * factor);

            var amountRequired = (int)Math.Round(input.Quantity * factor);

            // first use from store
            var amountDepleted = store.DepleteIfAvailable(input.Chemical, amountRequired);
            amountRequired -= amountDepleted;
            ////amountToProduce -= amountDepleted;

            // then, produce any of the left over
            if (amountRequired > 0)
            {
                // rs-todo: are separate ChemicalOutput and Input classes needed?
                Produce(new ChemicalOutput(input.Quantity, input.Chemical), amountRequired, /*, amountRequired*/ store, totalProduce);

                //if (input.Chemical == ORE)
                //{
                //    // Ore is a raw material and is not produced by a reaction
                //    var numberOfOreInputsRequired = (int) Math.Ceiling(amountToProduce / (decimal) input.Quantity);
                //    var oreRequired = input.Quantity * numberOfOreInputsRequired;

                //    store.IncreaseQuantity(ORE, oreRequired);
                //    totalProduce.IncreaseQuantity(ORE, oreRequired);
                //}
                //else
                //{
                //    Produce(input.Chemical, amountToProduce, store, totalProduce);
                //}

                // use the newly produced chemicals, to complete our consumption
                amountDepleted = store.DepleteIfAvailable(input.Chemical, amountRequired);
                amountRequired -= amountDepleted;
            }

            // check!
            if (amountRequired > 0)
            {
                throw new InvalidOperationException("Consume should consume all of required!");
            }
        }

        //private void EnumerateChemicalTotalsRequired(string chemicalRequired, int timesRequired, Dictionary<string, int> chemicalTotalsRequired)
        //{
        //    if (chemicalRequired != ORE)
        //    {
        //        var reaction = reactions[chemicalRequired];

        //        foreach (var inputChemicalQuantity in reaction.Inputs)
        //        {
        //            var inputChemical = inputChemicalQuantity.Chemical;
        //            var inputQuantity = inputChemicalQuantity.Quantity;

        //            if (!chemicalTotalsRequired.ContainsKey(inputChemical))
        //            {
        //                chemicalTotalsRequired.Add(inputChemical, 0);
        //            }

        //            chemicalTotalsRequired[inputChemical] += inputQuantity * timesRequired;

        //            EnumerateChemicalTotalsRequired(inputChemical, inputQuantity, chemicalTotalsRequired);
        //        }
        //    }
        //}

        ////// KEY is chemical, VALUE is the total quantity currently used for that chemical
        ////private readonly Dictionary<string, int> chemicalQuantitiesRemaining = new Dictionary<string, int>(); 

        ////// KEY is the chemical, VALUE is the total quantities that were produced for that chemical
        ////private readonly Dictionary<string, int> chemicalQuantitiesProduced = new Dictionary<string, int>();

        ////private int GetRemaining(string chemical) => chemicalQuantitiesRemaining.TryGetValue(chemical, out var remaining) ? remaining : 0;

        ////private void IncreaseRemaining(string chemical, int quantity)
        ////{
        ////    if (quantity == 0)
        ////    {
        ////        throw new InvalidOperationException("Invalid operation, remaining should never be updated with a zero amount.");
        ////    }

        ////    if (quantity > 0)
        ////    {
        ////        if (!chemicalQuantitiesRemaining.ContainsKey(chemical))
        ////        {
        ////            chemicalQuantitiesRemaining[chemical] = 0;
        ////        }

        ////        chemicalQuantitiesRemaining[chemical] += quantity;
        ////    }
        ////}

        ////private void Use(string chemicalRequired, int quantityRequired)
        ////{
        ////    var quantityRemaining = chemicalQuantitiesRemaining[chemicalRequired];

        ////    if (quantityRemaining < quantityRequired)
        ////    {
        ////        throw new InvalidOperationException("Invalid operation, UseRemaining should only be called once we know there is enough");
        ////    }

        ////    quantityRemaining -= quantityRequired;

        ////    if (quantityRemaining == 0)
        ////    {
        ////        chemicalQuantitiesRemaining.Remove(chemicalRequired);
        ////    }
        ////    else
        ////    {
        ////        chemicalQuantitiesRemaining[chemicalRequired] = quantityRemaining;
        ////    }
        ////}

        ////private void IncreaseQuantityProduced(string chemical, int quantityProducedIncrease)
        ////{
        ////    if (!chemicalQuantitiesProduced.ContainsKey(chemical))
        ////    {
        ////        chemicalQuantitiesProduced[chemical] = 0;
        ////    }

        ////    chemicalQuantitiesProduced[chemical] += quantityProducedIncrease;
        ////}

        ////private void Produce(string chemicalRequired, in int quantityRequired)
        ////{
        ////    var reaction = chemicalRequired == ORE
        ////        ? new Reaction(new ChemicalQuantity(quantityRequired, ORE))
        ////        : reactions[chemicalRequired];

        ////    // Only produce what we don't have in store, i.e. produce some more if we need to
        ////    var quantityInStore = GetRemaining(chemicalRequired);
        ////    var quantityToProduce = quantityRequired - quantityInStore;

        ////    if (quantityToProduce > 0)
        ////    {
        ////        var numReactionsRequired = (int)Math.Ceiling(quantityToProduce / (decimal) reaction.Output.Quantity);

        ////        for (var reactionNumber = 0; reactionNumber < numReactionsRequired; reactionNumber++)
        ////        {
        ////            // "Perform" the reaction, to produce this reaction's output
        ////            foreach (var reactionInput in reaction.Inputs)
        ////            {
        ////                Produce(reactionInput.Chemical, reactionInput.Quantity);
        ////                Use(reactionInput.Chemical, reactionInput.Quantity);
        ////            }
        ////        }

        ////        var quantityProduced = reaction.Output.Quantity * numReactionsRequired;

        ////        IncreaseRemaining(reaction.Output.Chemical, quantityProduced);
        ////        IncreaseQuantityProduced(reaction.Output.Chemical, quantityProduced);
        ////    }

        ////    // rs-todo: now, we need to update the quantities, proportionally
            
        ////}

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
                var output = new ChemicalOutput(int.Parse(match.Groups["quanity"].Value), match.Groups["chemical"].Value);

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
                            .Select(inMatch => new ChemicalInput(int.Parse(inMatch.Groups["quanity"].Value), inMatch.Groups["chemical"].Value))
                            .ToArray()));
            }

            return result;
        }

        public static NanoFactory Create(string input) => new NanoFactory(ParseReactions(input));
    }
}
