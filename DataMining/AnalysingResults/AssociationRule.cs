﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysingResults
{
    class AssociationRule
    {
        public string Principal { get; set; }
        public string[] Members { get; set; }
        public string Rule { get; set; }
        public double Support { get; set; }
        public double Confidence { get; set; }
        public double Lift { get; set; }
        public int Count { get; set; }
        public int TimeStamp { get; set; }
    }
}