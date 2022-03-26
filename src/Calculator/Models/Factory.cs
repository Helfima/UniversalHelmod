using Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Models
{
    public class Factory : BaseItem
    {
        public double Speed { get; set; }

        public double PowerConsumption { get; set; }
        public double PowerConsumptionExponent { get; set; }
        public double PowerProduction { get; set; }
        public double PowerProductionExponent { get; set; }
        public double Max { get; set; }
        public double Rate { get; set; }
        public string Category { get; set; }
        public bool IsMiner { get; set; }
        public ItemForm AllowedResourceForms { get; set; }
        public List<string> AllowedResources { get; set; }
        public double ExtractCycleTime { get; set; }
        public double ItemsPerCycle { get; set; }
    }
}
