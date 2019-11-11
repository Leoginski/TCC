using HtmlAgilityPack;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AnalysingResults
{
    class Program
    {
        public static string inputPath = $@"{ConfigurationManager.AppSettings["InputPath"]}";
        public static string outputPath = $@"{ConfigurationManager.AppSettings["OutputPath"]}";

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
            IEnumerable<AssociationRule> ruleSet = new List<AssociationRule>();

            foreach (var dir in Directory.GetDirectories(inputPath))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    var list = File.ReadAllLines(file);

                    string timeStamp = Regex.Match(file, @"(\d{6})").Groups[1].Value;

                    ruleSet = ruleSet.Concat(GetAssociationRules(list, timeStamp));
                }
            }

            ruleSet = ruleSet.Where(x => x.Confidence >= 1).ToList();

            var grouped = ruleSet.GroupBy(x => x.Principal)
                .ToDictionary(x => x.Key, x => x.Select(e => e).ToList())
                .ToList();

            Console.WriteLine($"Starting requests with {grouped.Count()} companies.");

            var resultsByKey = new Dictionary<string, List<SearchResult>>();

            foreach (var item in grouped)
            {
                var result = JournalSearch(item.Key);

                if (result.Count() > 0)
                {
                    resultsByKey.Add(key: item.Key, value: result);
                }
            }

            var foundCompanies = resultsByKey.Count();
            Console.WriteLine($"Total Companies on search results: {resultsByKey.Count()}");

            if (resultsByKey.Any(x => x.Value.Count() > 0))
            {
                ruleSet = ruleSet.Where(x => resultsByKey.ContainsKey(x.Principal));
                SaveRules(ruleSet);
                SaveResults(resultsByKey);
            }
        }

        private static void SaveRules(IEnumerable<AssociationRule> searched)
        {
            using (var fs = new FileStream($@"{outputPath}\ruleSet.csv", FileMode.Append))
            {
                var fw = new StreamWriter(fs, Encoding.Default);

                fw.WriteLine(
                    $"Principal;" +
                    $"TimeStamp;" +
                    $"Rule;" +
                    $"Support;" +
                    $"Confidence;" +
                    $"Lift;" +
                    $"Count;"
                );

                foreach (var rule in searched)
                {
                    fw.WriteLine(
                        $"{rule.Principal};" +
                        $"{rule.TimeStamp};" +
                        $"{rule.Rule};" +
                        $"{rule.Support};" +
                        $"{rule.Confidence};" +
                        $"{rule.Lift};" +
                        $"{rule.Count};"

                    );
                }
            }
        }

        private static void SaveResults(Dictionary<string, List<SearchResult>> foundResults)
        {
            using (var fs = new FileStream($@"{outputPath}\foundResults.csv", FileMode.Append))
            {
                var fw = new StreamWriter(fs, Encoding.Default);

                fw.WriteLine(
                    $"Principal;" +
                    $"UsedTerm;" +
                    $"Url;" +
                    $"Title;" +
                    $"Abstract;" +
                    $"DataPublicacao"
                );

                foreach (var item in foundResults)
                {
                    foreach (var result in item.Value)
                    {
                        fw.WriteLine(
                        $"{item.Key};" +
                        $"{result.UsedTerm};" +
                        $"{result.Url};" +
                        $"{result.Title};" +
                        $"{result.Abstract};" +
                        $"{result.DataPublicacao}"
                        );
                    }
                }
            }
        }

        private static List<SearchResult> JournalSearch(string key)
        {
            var url = "https://www.jusbrasil.com.br/busca?q=";
            var termos = new List<string>()
            {
                "tcu+fraude",
                "tcu+licitação",
                "tcu+ilicito",
                "tcu+lavagem",
                "tcu+conluio",
                "tcu+cartel",
                "tcu+crime",
            };

            var results = new List<SearchResult>();

            foreach (var termo in termos)
            {
                string searchTerm = $"{key}+{termo}";
                string target = $"{url}{searchTerm}";
                Console.WriteLine(target);

                DoSearch(results, target, searchTerm);
            }

            return results;
        }

        private static void DoSearch(List<SearchResult> results, string target, string searchTerm)
        {
            var web = new HtmlWeb();
            var doc = web.Load(target);

            if (!ParseResults(results, searchTerm, doc))
            {
                DoSearch(results, target, searchTerm);
            }
        }

        private static bool ParseResults(List<SearchResult> results, string searchTerm, HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//div[@class = 'SearchResults-documents']/div/div");
            if (nodes != null)
            {
                Console.WriteLine($"{searchTerm}: {nodes.Count()} results");

                foreach (var node in nodes)
                {
                    var nodeTitle = node.SelectSingleNode("./h2/a");
                    var nodePublicacao = node.SelectSingleNode("./div[@class = 'BaseSnippetWrapper-publisher']/span[@class = 'BaseSnippetWrapper-highlight-date']");
                    var nodeBody = node.SelectSingleNode("./div[@class = 'BaseSnippetWrapper-body']");

                    var result = new SearchResult
                    {
                        Title = nodeTitle.InnerText,
                        Url = nodeTitle.GetAttributeValue("href", string.Empty),
                        DataPublicacao = nodePublicacao != null ? nodePublicacao.InnerText.Split(':')[1].Trim() : string.Empty,
                        Abstract = nodeBody.InnerText,
                        UsedTerm = searchTerm
                    };

                    results.Add(result);
                }

                return true;
            }

            if (doc.DocumentNode.SelectSingleNode("//span[contains(text(), 'não encontrou nenhum documento')]") != null)
            {
                Console.WriteLine($"{searchTerm}: 0 results");
                return true;
            }

            return false;
        }

        private static IEnumerable<AssociationRule> GetAssociationRules(string[] list, string timeStamp)
        {
            return list.Skip(1).Select(x => ParseAssociationRule(x.Split(';'), int.Parse(timeStamp)));
        }

        private static AssociationRule ParseAssociationRule(string[] fields, int timeStamp)
        {
            var ruleItens = fields[0].Split('>');

            var rule = new AssociationRule
            {
                Principal = Regex.Replace(ruleItens[1], @"\D", string.Empty),
                Members = ruleItens[0].Split(',').Select(x => Regex.Replace(x, @"\D", string.Empty)).ToArray(),
                Rule = fields[0],
                Support = double.Parse(fields[1], CultureInfo.InvariantCulture),
                Confidence = double.Parse(fields[2], CultureInfo.InvariantCulture),
                Lift = double.Parse(fields[3], CultureInfo.InvariantCulture),
                Count = int.Parse(fields[4], CultureInfo.InvariantCulture),
                TimeStamp = timeStamp
            };

            return rule;
        }
    }
}
