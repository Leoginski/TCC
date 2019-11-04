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
            CheckPK();

            //CheckColumnExpectedLength("Código Item Compra", 23);

            Dictionary<string, List<string>> filesByYear = new Dictionary<string, List<string>>();

            foreach (var filename in Directory.GetFiles(dataSetPath, @"*Participantes*"))
            {
                var year = Regex.Match(filename, @"(\d{4})\d{2}_").Groups[1].Value;

                if (filesByYear.ContainsKey(year))
                    filesByYear[year].Add(filename);
                else
                    filesByYear.Add(year, new List<string>() { filename });
            }

            foreach (var year in filesByYear)
            {
                List<string> yearEvents = new List<string>();

                foreach (var file in year.Value)
                {
                    var events = GetEventList(file);


                }



                File.WriteAllLines($@"{dataSetPath}{year}_dataser.csv", yearEvents.ToArray());
            }
        }

        private static void CheckPK()
        {
            IEnumerable<string> fullList = new List<string>();
            foreach (var filename in Directory.GetFiles(dataSetPath, @"*Participantes*"))
            {
                var nextMonth = GetValidCodItemList(filename);
                fullList = fullList.Concat<string>(nextMonth);
            }

            var fullCount = fullList.Count();

            var uniqueCount = fullList.Distinct().Count();

            if (fullCount != uniqueCount)
            {
                throw new Exception("A coluna não é PK.");
            }
        }

        private static void CheckColumnExpectedLength(string column, int expectedLength)
        {
            foreach (var filename in Directory.GetFiles(dataSetPath, @"*Participantes*"))
            {
                string[] header = GetHeader(filename);
                int codItemIndex = GetFieldIndex(header, column);

                var invalidCodItemCompra = File.ReadLines(filename, Encoding.Default).Skip(1)
                    .First(x => x.Split(delimiter)[codItemIndex].Trim('\"').Length != expectedLength);

                if (!string.IsNullOrEmpty(invalidCodItemCompra))
                {
                    throw new Exception($"File: \"{filename}\" contains column \"{column}\" with invalid length.");
                }
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

                    return (codItem.Length == 23 && !cnpj.Equals("-11"));
                }).Select(x =>
                {
                    return $"{x.Split(delimiter)[codItemIndex].Trim('\"')}"; //{fileId}
                });

            return csv.Distinct();
        }

        private static IEnumerable<Participante> GetEventList(string path)
        {
            string[] header = GetHeader(path);

            var fileId = Regex.Match(path, @"(\d{6})_").Groups[1].Value;

            int codItemIndex = GetFieldIndex(header, "Código Item Compra");
            int cnpjIndex = GetFieldIndex(header, "CNPJ Participante");
            int flagIndex = GetFieldIndex(header, "Flag Vencedor");

            IEnumerable<Participante> csv = File.ReadLines(path, Encoding.Default).Skip(1)
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

            return csv.Distinct();
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