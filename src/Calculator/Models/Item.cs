using Calculator.Enums;
using Calculator.Protos;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Models
{
    public class Item : BaseItem
    {
        public int StackSize { get; set; }
        public double EnergyValue { get; set; }
        public double RadioactiveDecay { get; set; }
        public int ResourceSinkPoints { get; set; }
        public string Form { get; set; }
        public ItemCost ItemCost { get; set; }
        public List<Item> WhereUsed { get; set; } = new List<Item>();


    }
}
