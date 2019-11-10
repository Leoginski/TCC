using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysingResults
{
    class AssociationRule
    {
        public string Rule { get; set; }
        public double Support { get; set; }
        public double Confidence { get; set; }
        public double Lift { get; set; }
        public int Count { get; set; }
        public int TimeStamp { get; set; }
    }
}
