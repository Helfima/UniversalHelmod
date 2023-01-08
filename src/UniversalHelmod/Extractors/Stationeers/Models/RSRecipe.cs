using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Extractors.Stationeers.Models
{
    public class RSRecipe : RSElement
    {
        public double Time { get; set; }
        public int Tier { get; set; } = 1;
        public int Energy { get; set; }
        public string Type { get; set; }
        public List<RSAmount> Products { get; set; } = new List<RSAmount>();
        public List<RSAmount> Ingredients { get; set; } = new List<RSAmount>();
        public HashSet<string> MadeIn { get; set; } = new HashSet<string>();
    }
}
