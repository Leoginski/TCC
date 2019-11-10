using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalysingResults
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetDataSetByCNPJ();
            //GetDataSetByMonth();
            RuleBreakingAnalyses();
        }

        private static void GetDataSetByCNPJ()
        {
            //88.309.620/0002-39 - ALSTOM BRASIL ENERGIA
            //00.811.185/0001-14 - BOMBARDIER - BOMBARDIER TRANSPORTATION BRASIL LTDA
            //02.430.238/0006-97 - CAF BRASIL INDUSTRIA E COMERCIO SA
            //67.151.258/0001-60 - MGE - EQUIPAMENTOS E SERVICOS FERROVIARIOS LTDA
            //29.918.943/0008-56 - IESA PROJETOS, EQUIPAMENTOS E MONTAGENS S/A.
            //61.139.697/0001-70 - MITSUI & CO. (BRASIL) S.A.
            //31.876.709/0001-89 - MPE MONTAGENS E PROJETOS ESPECIAIS S/A
            //03.652.914/0001-25 - Tc/br - Tecnologia e Consultoria Brasileira LTDA
            //02.249.216/0001-10 - Trans Sistemas De Transportes S A
            //61.288.437/0001-67 - EMPRESA TEJOFRAN DE SANEAMENTO E SERVICOS LTDA
            //02.587.355/0001-54 - TEMOINSA DO BRASIL LTDA
            var dictionary = new Dictionary<string, string>()
            {
                {"88309620000239", "ALSTOM BRASIL ENERGIA"},
                {"00811185000114", "BOMBARDIER - BOMBARDIER TRANSPORTATION BRASIL LTDA"},
                {"02430238000697", "CAF BRASIL INDUSTRIA E COMERCIO SA"},
                {"67151258000160", "MGE - EQUIPAMENTOS E SERVICOS FERROVIARIOS LTDA"},
                {"29918943000856", "IESA PROJETOS, EQUIPAMENTOS E MONTAGENS S/A."},
                {"61139697000170", "MITSUI & CO. (BRASIL) S.A."},
                {"31876709000189", "MPE MONTAGENS E PROJETOS ESPECIAIS S/A"},
                {"03652914000125", "Tc/br - Tecnologia e Consultoria Brasileira LTDA"},
                {"02249216000110", "Trans Sistemas De Transportes S A"},
                {"61288437000167", "EMPRESA TEJOFRAN DE SANEAMENTO E SERVICOS LTDA"},
                {"02587355000154", "TEMOINSA DO BRASIL LTDA"}
            };

            IEnumerable<string> eventList = new List<string>();
            IEnumerable<string> result = new List<string>();
            //Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            foreach (var file in Directory.GetFiles(@"C:\Users\leosm\Documents\Projects\TCC\DataSet", "*_dataset.csv"))
            {
                var list = File.ReadAllLines(file);
                eventList = eventList.Concat(list.Where(x => dictionary.ContainsKey(x.Split(';')[1])).Select(x => x.Split(';')[0])).ToList();

                result = result.Concat(list.Where(x => eventList.Contains(x.Split(';')[0]))).ToList();
            }


            File.WriteAllLines(@"C:\Users\leosm\Documents\Projects\TCC\cartelFull.csv", result);

            var coditens = eventList.Select(x => x.Split(';')[0]).Distinct().Count();
        }

        private static void GetDataSetByMonth()
        {
            string path = @"C:\Users\leosm\Documents\Projects\TCC\RulesByMonth";

            foreach (var file in Directory.GetFiles(@"C:\Users\leosm\Documents\Projects\TCC\DataSet", "*_dataset.csv"))
            {
                var matchYear = Regex.Match(file, @"(\d{4})_dataset");

                if (!matchYear.Success)
                    throw new Exception("Invalid filname.");

                var list = File.ReadAllLines(file);

                var monthGroups = list.GroupBy(x => x.Split(';')[0].Split('.')[1])
                .ToDictionary(x => x.Key, x => x.Select(e => e).ToList())
                .ToList();

                foreach (var yearMonth in monthGroups)
                {
                    using (var fw = new StreamWriter($@"{path}\{matchYear.Groups[1].Value}\{yearMonth.Key}.csv", true))
                    {
                        foreach (var line in yearMonth.Value)
                        {
                            fw.WriteLine(line);
                        }
                    }
                }
            }
        }

        private static void RuleBreakingAnalyses()
        {
            string path = @"C:\Users\leosm\Documents\Projects\TCC\RulesByMonth";

            IEnumerable<AssociationRule> fullRuleSet = new List<AssociationRule>();

            for (int year = 2013; year < 2019; year++)
            {

                foreach (var file in Directory.GetFiles($@"{path}\{year}\", "*_rules.csv"))
                {
                    var list = File.ReadAllLines(file);
                    fullRuleSet = fullRuleSet.Concat(GetAssociationRules(list, file)).ToList();
                }
            }

            fullRuleSet = fullRuleSet.OrderBy(x => x.Confidence).ToList();

            var hundredPercent = fullRuleSet.Where(x => x.Confidence >= 1).ToList().OrderBy(x => x.Confidence);


        }

        private static IEnumerable<AssociationRule> GetAssociationRules(string[] list, string file)
        {
            int timeStamp = int.Parse(Regex.Match(file, @"(\d{6})_rules").Groups[1].Value);
            return list.Skip(1).Select(x => ParseAssociationRule(x.Split(';'), timeStamp));
        }

        private static AssociationRule ParseAssociationRule(string[] fields, int timeStamp)
        {
            return new AssociationRule
            {
                Rule = fields[0],
                Support = double.Parse(fields[1], CultureInfo.InvariantCulture),
                Confidence = double.Parse(fields[2], CultureInfo.InvariantCulture),
                Lift = double.Parse(fields[3], CultureInfo.InvariantCulture),
                Count = int.Parse(fields[4], CultureInfo.InvariantCulture),
                TimeStamp = timeStamp
            };
        }
    }
}
