using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ValidatePK.Models;

namespace ValidatePK
{
    class Program
    {
        private static char delimiter = ';';
        private static string dataSetPath = @"C:\Users\leosm\Documents\Projects\TCC\DataSet\";

        static void Main(string[] args)
        {
            var events = File.ReadAllLines(@"C:\Users\leosm\Documents\Projects\TCC\DataSet\2019_dataset.csv");

            var list = events.Select(x =>
            {
                var split = x.Split(';');
                return new Participante
                {
                    CodItemCompra = split[0],
                    CnpjParticipante = split[1]
                };
            });

            var foundOnRules = list.GroupBy(x => x.CodItemCompra)
                .ToDictionary(x => x.Key, x => x.Select(e => e.CnpjParticipante).ToList())
                //.Where(x => x.CnpjParticipante.Equals("26889274000177") || x.CnpjParticipante.Equals("30223033000161"))
                .ToList();

            var filter = foundOnRules.Where(x => x.Value.Contains("26889274000177") && x.Value.Contains("30223033000161")).ToList();
            //CheckPK();
            //GenerateDataSet();
        }

        private static void GenerateDataSet()
        {
            Dictionary<string, List<string>> filesByYear = new Dictionary<string, List<string>>();

            foreach (var filename in Directory.GetFiles(dataSetPath, @"*Participantes*"))
            {
                var year = Regex.Match(filename, @"(\d{4})\d{2}_").Groups[1].Value;

                if (filesByYear.ContainsKey(year))
                    filesByYear[year].Add(filename);
                else
                    filesByYear.Add(year, new List<string>() { filename });
            }

            foreach (var year in filesByYear.Skip(1))
            {
                IEnumerable<Participante> yearEvents = new List<Participante>();

                Console.WriteLine($"Concating {year.Key}");
                foreach (var file in year.Value)
                {
                    yearEvents = yearEvents.Concat(GetParticipanteList(file));
                }

                Console.WriteLine($"Grouping {year.Key}");
                var yearGroup = yearEvents.GroupBy(x => x.CodItemCompra).ToList();

                Console.WriteLine($"Removing {year.Key}");
                yearGroup.RemoveAll(x => IsInvalidEvent(x));

                Console.WriteLine($"Writing {year.Key}");
                using (var fw = new StreamWriter($@"{dataSetPath}{year.Key}_dataset.csv", true))
                {
                    foreach (var group in yearGroup)
                    {
                        foreach (var participante in group)
                        {
                            fw.WriteLine($"{participante.CodItemCompra};{participante.CnpjParticipante}");
                        }
                    }
                }
            }
        }

        private static bool IsInvalidEvent(IEnumerable<Participante> participantes)
        {
            return participantes.Where(x => x.FlagVencedor.ToLower().Equals("sim")).Count() != 1
                || participantes.Where(x => x.FlagVencedor.ToLower().Equals("não")).Count() < 1;
        }

        private static void CheckPK()
        {
            IEnumerable<string> fullList = new List<string>();
            foreach (var filename in Directory.GetFiles(dataSetPath, @"*Participantes*"))
            {
                var nextMonth = GetValidCodItemList(filename);
                fullList = fullList.Concat<string>(nextMonth);
            }

            fullList = fullList.ToList();

            var fullCount = fullList.Count();
            var uniqueCount = fullList.Distinct().Count();

            if (fullCount != uniqueCount)
            {
                throw new Exception("A coluna não é PK.");
            }
        }

        private static IEnumerable<string> GetValidCodItemList(string path)
        {
            string[] header = GetHeader(path);

            var fileId = Regex.Match(path, @"(\d{6})_").Groups[1].Value;

            int codItemIndex = GetFieldIndex(header, "Código Item Compra");
            int cnpjIndex = GetFieldIndex(header, "CNPJ Participante");
            int flagIndex = GetFieldIndex(header, "Flag Vencedor");

            IEnumerable<string> csv = File.ReadLines(path, Encoding.Default).Skip(1)
                .Where(x =>
                {
                    var fields = x.Split(delimiter);

                    var codItem = fields[codItemIndex].Trim('\"');
                    var cnpj = fields[cnpjIndex].Trim('\"');

                    return (IsValidEvent(codItem, cnpj));
                }).Select(x =>
                {
                    return $"{x.Split(delimiter)[codItemIndex].Trim('\"')}{fileId}";
                });

            return csv.Distinct();
        }

        private static bool IsValidEvent(string codItem, string cnpj)
        {
            return Regex.Match(codItem, @"\d{21,23}.").Success && !cnpj.Contains("-1");
        }

        private static IEnumerable<Participante> GetParticipanteList(string path)
        {
            string[] header = GetHeader(path);

            var fileId = Regex.Match(path, @"(\d{6})_").Groups[1].Value;

            int codItemIndex = GetFieldIndex(header, "Código Item Compra");
            int cnpjIndex = GetFieldIndex(header, "CNPJ Participante");
            int flagIndex = GetFieldIndex(header, "Flag Vencedor");

            IEnumerable<Participante> list = File.ReadLines(path, Encoding.Default).Skip(2)
                .Select(x =>
                {
                    var columns = x.Split(delimiter);

                    return new Participante()
                    {
                        CodItemCompra = $"{columns[codItemIndex].Trim('\"')}{fileId}",
                        CnpjParticipante = columns[cnpjIndex].Trim('\"'),
                        FlagVencedor = columns[flagIndex].Trim('\"')
                    };
                });

            return list.Where(x => IsValidEvent(x.CodItemCompra, x.CnpjParticipante));
        }

        private static string[] GetHeader(string path)
        {
            return File.ReadLines(path, Encoding.Default).First().Split(delimiter).Select(x => x.Trim('\"')).ToArray();
        }

        private static int GetFieldIndex(string[] header, string field)
        {
            int codItemIndex = Array.IndexOf(header, field);

            if (codItemIndex == -1)
                throw new Exception($"Field \"{field}\" not found");

            return codItemIndex;
        }
    }
}