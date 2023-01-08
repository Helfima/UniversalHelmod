using UniversalHelmod.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Databases.Converter
{
    public class JsonFactory : JsonBaseItem
    {
        public double Speed { get; set; }
        public double PowerConsumption { get; set; }
        public double PowerConsumptionExponent { get; set; }
        public double PowerProduction { get; set; }
        public double PowerStorage { get; set; }
        public double PowerProductionExponent { get; set; }
        public List<string> AllowedResourceForms { get; set; } = new List<string>();
        public List<string> AllowedResources { get; set; } = new List<string>();
    }
}
