using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Protos.FGProtos
{
    public class FGExtractor : FGFactory
    {
        public object AllowedResourceForms { get; set; }
        public object AllowedResources { get; set; }
        public double ExtractCycleTime { get; set; }
        public double ItemsPerCycle { get; set; }
    }
}
