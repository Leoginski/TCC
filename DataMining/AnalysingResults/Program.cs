using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysingResults
{
    class Program
    {
        static void Main(string[] args)
        {
            //31.876.709 / 0001 - 89
            //31876709000189
            IEnumerable<string> mpeList = new List<string>();
            IEnumerable<string> result = new List<string>();
            //var result
            foreach (var file in Directory.GetFiles(@"C:\Users\leosm\Documents\Projects\TCC\DataSet", "*_dataset.csv"))
            {
                var list = File.ReadAllLines(file);
                mpeList = mpeList.Concat(list.Where(x => x.Contains("31876709000189")).Select(x => x.Split(';')[0])).ToList();

                result = result.Concat(list.Where(x => mpeList.Contains(x.Split(';')[0]))).ToList();
            }

            File.WriteAllLines(@"C:\Users\leosm\Documents\Projects\TCC\mpeFull.csv", result);

            var coditens = mpeList.Select(x => x.Split(';')[0]).Distinct();
        }
    }
}
