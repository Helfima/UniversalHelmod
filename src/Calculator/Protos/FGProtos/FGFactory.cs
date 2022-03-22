using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Protos.FGProtos
{
    public class FGFactory : FGItem
    {
        public double Speed { get; set; }
        public double PowerConsumption { get; set; }
        public double PowerConsumptionExponent { get; set; }
        public double PowerProduction { get; set; }
        public double PowerProductionExponent { get; set; }
        public object AllowedResourceForms { get; set; }
        public object AllowedResources { get; set; }
    }
}
