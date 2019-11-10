using Accord.MachineLearning.Rules;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ValidatePK.Models;

namespace RunApriori
{
    class Program
    {
        static void Main(string[] args)
        {
            var events = File.ReadAllLines(@"C:\Users\leosm\Documents\Projects\TCC\DataSetByCNPJ\cartelFull.csv");

            var list = events.Select(x =>
            {
                var split = x.Split(';');
                return new Participante
                {
                    CodItemCompra = split[0],
                    CnpjParticipante = split[1]
                };
            });

            var groups = list.GroupBy(x => x.CodItemCompra).ToDictionary(x => x.Key, x => x.Select(e => e.CnpjParticipante).ToArray()).ToArray();

            var dataset = groups.Select(x => x.Value.ToArray()).ToArray();

            // Create a new A-priori learning algorithm with the requirements
            var apriori = new Apriori<string>(threshold: 3, confidence: 0.7);

            // Use apriori to generate a n-itemset generation frequent pattern
            AssociationRuleMatcher<string> classifier = apriori.Learn(dataset);

            // Generate association rules from the itemsets:
            AssociationRule<string>[] rules = classifier.Rules;
        }
    }
}
