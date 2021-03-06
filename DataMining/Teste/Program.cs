﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
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

            for (int year = 2013; year < 2019; year++)
            {
                foreach (var file in Directory.GetFiles($@"{inputPath}\{year}\", "*_rules.csv"))
                {
                    //IEnumerable<AssociationRule> ruleSet = new List<AssociationRule>();
                    var list = File.ReadAllLines(file);
                    string timeStamp = Regex.Match(file, @"(\d{6})_rules").Groups[1].Value;

                    //ruleSet = GetAssociationRules(list, timeStamp);
                    ruleSet = ruleSet.Concat(GetAssociationRules(list, timeStamp));
                    //ruleSet = ruleSet.Where(x => x.Confidence >= 1).OrderBy(x => x.Support).ToList();

                    //JournalSearch(ruleSet);
                    //var searched = ruleSet.Where(x => x.Results != null && x.Results.Count > 0);

                    //if (searched.Count() > 0)
                    //{
                    //    SaveResults(searched, timeStamp);
                    //}
                }
            }

            ruleSet = ruleSet.Where(x => x.Confidence >= 1).OrderBy(x => x.Support).ToList();

            var grouped = ruleSet.Where(x => x.Confidence >= 1).GroupBy(x => Regex.Replace(x.Rule.Split('>')[1], @"\D", string.Empty))
                .ToDictionary(x => x.Key, x => x.Select(e => e).ToList())
                .ToList();

            Console.WriteLine($"Starting requests with {ruleSet.Count()} rules.");

            JournalSearch(ruleSet);

            var searched = ruleSet.Where(x => x.Results != null && x.Results.Count > 0);

            Console.WriteLine($"TotalResults: {searched.Count()}");

            if (searched.Count() > 0)
            {
                SaveResults(searched);
            }
        }

        private static void SaveResults(IEnumerable<AssociationRule> searched)
        {
            using (var fs = new FileStream($@"{outputPath}\result.csv", FileMode.Append))
            {
                var fw = new StreamWriter(fs, Encoding.Default);

                fw.WriteLine(
                    $"TimeStamp;" +
                    $"Rule;" +
                    $"Support;" +
                    $"Confidence;" +
                    $"Lift;" +
                    $"Count;" +
                    $"UsedTerm;" +
                    $"Url;" +
                    $"Title;" +
                    $"Abstract;" +
                    $"DataPublicacao"
                );

                foreach (var rule in searched)
                {
                    foreach (var result in rule.Results)
                    {
                        fw.WriteLine(
                            $"{rule.TimeStamp};" +
                            $"{rule.Rule};" +
                            $"{rule.Support};" +
                            $"{rule.Confidence};" +
                            $"{rule.Lift};" +
                            $"{rule.Count};" +
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

        private static void JournalSearch(IEnumerable<AssociationRule> hundredPercent)
        {
            var termos = new List<string>()
            {
                "tcu+fraude",
                "tcu+licitação",
                "tcu+ilicito",
                "tcu+lavagem",
                "tcu+conluio",
                "tcu+cartel",
            };

            var url = "https://www.jusbrasil.com.br/busca?q=";
            foreach (var association in hundredPercent)
            {
                association.Results = new List<SearchResult>();

                foreach (var termo in termos)
                {
                    string principal = Regex.Replace(association.Rule.Split('>')[1], @"\D", string.Empty);
                    string searchTerm = $"{principal}+{termo}";

                    string target = $"{url}{searchTerm}";
                    Console.WriteLine(target);

                    var web = new HtmlWeb();
                    var doc = web.Load(target);

                    ParseResults(association, searchTerm, doc);
                }
            }
        }

        private static void ParseResults(AssociationRule association, string searchTerm, HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//div[@class = 'SearchResults-documents']/div/div");
            if (nodes != null)
            {
                Console.WriteLine($"{association.Rule} - {searchTerm}: {nodes.Count()} results");

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

                    association.Results.Add(result);
                }
            }
            //else if (doc.DocumentNode.SelectSingleNode("//span[contains(text(), 'não encontrou nenhum documento')]") == null)
            //{
            //    throw new Exception("Erro inesperado, possível bloqueio de IP.");
            //}
        }

        private static IEnumerable<AssociationRule> GetAssociationRules(string[] list, string timeStamp)
        {
            return list.Skip(1).Select(x => ParseAssociationRule(x.Split(';'), int.Parse(timeStamp)));
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
