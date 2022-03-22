using Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Calculator.Protos.FGProtos
{
    public class FGRecipe : FGElement
    {
        public string FullName;
        public string DisplayName;
        public object Ingredients;
        public object Product;
        public double ManufacturingMenuPriority;
        public double ManufactoringDuration;
        public double ManualManufacturingMultiplier;
        public object ProducedIn;
        public string RelevantEvents;
        public double VariablePowerConsumptionConstant;
        public double VariablePowerConsumptionFactor;
        public FGRecipe(JsonElement element) : base(element)
        {
            FullName = element.GetStringValue("FullName");
            DisplayName = element.GetStringValue("mDisplayName");
            Ingredients = element.GetArrayValue("mIngredients");
            Product = element.GetArrayValue("mProduct");
            ManufacturingMenuPriority = element.GetDoubleValue("mManufacturingMenuPriority");
            ManufactoringDuration = element.GetDoubleValue("mManufactoringDuration");
            ManualManufacturingMultiplier = element.GetDoubleValue("mManualManufacturingMultiplier");
            ProducedIn = element.GetArrayValue("mProducedIn");
            RelevantEvents = element.GetStringValue("mRelevantEvents");
            VariablePowerConsumptionConstant = element.GetDoubleValue("mVariablePowerConsumptionConstant");
            VariablePowerConsumptionFactor = element.GetDoubleValue("mVariablePowerConsumptionFactor");
        }
    }
}
