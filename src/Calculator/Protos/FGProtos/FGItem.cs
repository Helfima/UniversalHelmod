using Calculator.Enums;
using Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows.Media.Imaging;

namespace Calculator.Protos.FGProtos
{
    public class FGItem : FGElement
    {
        public string DisplayName;
        public string Description;
        public string AbbreviatedDisplayName;
        public int StackSize;
        public bool CanBeDiscarded;
        public bool RememberPickUp;
        public double EnergyValue;
        public double RadioactiveDecay;
        public string Form;
        public string SmallIcon;
        public string PersistentBigIcon;
        public string SubCategories;
        public double MenuPriority;
        public string FluidColor;
        public string GasColor;
        public int ResourceSinkPoints;
        public double BuildMenuPriority;
        public FGItem(){}
        public FGItem(JsonElement element): base(element)
        {
            DisplayName = element.GetStringValue("mDisplayName");
            Description = element.GetStringValue("mDescription");
            AbbreviatedDisplayName = element.GetStringValue("mAbbreviatedDisplayName");
            StackSize = GetStackValue(element);
            CanBeDiscarded = element.GetBooleanValue("mCanBeDiscarded");
            RememberPickUp = element.GetBooleanValue("mRememberPickUp");
            EnergyValue = element.GetDoubleValue("mEnergyValue");
            RadioactiveDecay = element.GetDoubleValue("mRadioactiveDecay");
            Form = element.GetStringValue("mForm");
            SmallIcon = element.GetStringValue("mSmallIcon");
            PersistentBigIcon = element.GetStringValue("mPersistentBigIcon");
            SubCategories = element.GetStringValue("mSubCategories");
            MenuPriority = element.GetDoubleValue("mMenuPriority");
            FluidColor = element.GetStringValue("mFluidColor");
            GasColor = element.GetStringValue("mGasColor");
            ResourceSinkPoints = element.GetIntegerValue("mResourceSinkPoints");
            BuildMenuPriority = element.GetDoubleValue("mBuildMenuPriority");
        }

        protected int GetStackValue(JsonElement element)
        {
            string stack = element.GetStringValue("mStackSize");
            switch (stack)
            {
                case "SS_ONE":
                    return 1;
                case "SS_SMALL":
                    return 50;
                case "SS_MEDIUM":
                    return 100;
                case "SS_BIG":
                    return 200;
                case "SS_HUGE":
                    return 500;
                case "SS_FLUID":
                    return 50000;
                default:
                    return 0;
            }
        }

        public double Count { get; set; }
        public BitmapImage Icon { get; set; }
        public string ItemType = "Item";
        public T Clone<T>() where T : FGItem, new()
        {
            return new T()
            {
                ClassName = this.ClassName,
                DisplayName = this.DisplayName,
                Description = this.Description,
                AbbreviatedDisplayName = this.AbbreviatedDisplayName,
                StackSize = this.StackSize,
                CanBeDiscarded = this.CanBeDiscarded,
                RememberPickUp = this.RememberPickUp,
                EnergyValue = this.EnergyValue,
                RadioactiveDecay = this.RadioactiveDecay,
                Form = this.Form,
                SmallIcon = this.SmallIcon,
                PersistentBigIcon = this.PersistentBigIcon,
                SubCategories = this.SubCategories,
                MenuPriority = this.MenuPriority,
                FluidColor = this.FluidColor,
                GasColor = this.GasColor,
                ResourceSinkPoints = this.ResourceSinkPoints,
                BuildMenuPriority = this.BuildMenuPriority,
                Count = this.Count,
                Icon = this.Icon,
                ItemType = this.ItemType
            };
        }
    }
}
