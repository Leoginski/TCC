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
            var mpeList = new List<string>();
            foreach (var file in Directory.GetFiles(@"C:\Users\leosm\Documents\Projects\TCC\DataSet", "*2013_dataset.csv"))
            {
                var list = File.ReadAllLines(file);
                mpeList = mpeList.Concat(list.Where(x => x.Contains("31876709000189"))).ToList();
            }

            var coditens = mpeList.Select(x => x.Split(';')[0]).Distinct();

            foreach (var file in Directory.GetFiles(@"C:\Users\leosm\Documents\Projects\TCC\DataSet", "2013_dataset.csv"))
            {
                var list = File.ReadAllLines(file);
                var result = list.Where(x => coditens.Contains(x.Split(';')[0])).ToList();

                File.WriteAllLines(@"C:\Users\leosm\Documents\Projects\TCC\Dataset\mpe.csv", result);
                //.Where(x => coditens.Any(y => y.Equals(x.Split(';')[1]))).Select(x => x).ToList();
            }
        }
    }
}
