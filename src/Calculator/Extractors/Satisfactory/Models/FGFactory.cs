using Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Calculator.Extractors.Satisfactory.Models
{
    public class FGFactory : FGBaseOnItem
    {
        public double Speed { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public double PowerConsumption { get; set; }
        public double PowerConsumptionExponent { get; set; }
        public double PowerProduction { get; set; }
        public double PowerProductionExponent { get; set; }
        public object AllowedResourceForms { get; set; }
        public object AllowedResources { get; set; }
        public double ExtractCycleTime { get; set; }
        public double ItemsPerCycle { get; set; }
        public FGFactory(FGItem item, string type, JsonElement element)
        {
            Item = item;
            Type = type;
            DisplayName = element.GetStringValue("mDisplayName");
            Description = element.GetStringValue("mDescription");
            Speed = element.GetDoubleValue("mManufacturingSpeed");
            PowerConsumption = element.GetDoubleValue("mPowerConsumption");
            PowerConsumptionExponent = element.GetDoubleValue("mPowerConsumptionExponent");
            PowerProduction = element.GetDoubleValue("mPowerProduction");
            PowerProductionExponent = element.GetDoubleValue("mPowerProductionExponent");
            // specifique extractor
            AllowedResourceForms = element.GetArrayValue("mAllowedResourceForms");
            AllowedResources = element.GetArrayValue("mAllowedResources");
            ExtractCycleTime = element.GetDoubleValue("mExtractCycleTime");
            ItemsPerCycle = element.GetDoubleValue("mItemsPerCycle");
        }
    }
}
