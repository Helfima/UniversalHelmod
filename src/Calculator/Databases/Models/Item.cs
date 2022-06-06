using Calculator.Enums;
using Calculator.Extractors;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Databases.Models
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

        public Item Clone()
        {
            var item = new Item()
            {
                Name = this.Name,
                DisplayName = this.DisplayName,
                Description = this.Description,
                ItemType = this.ItemType,
                IconPath = this.IconPath,
                StackSize = this.StackSize,
                EnergyValue = this.EnergyValue,
                Form = this.Form,
            };
            return item;
        }
    }
}
