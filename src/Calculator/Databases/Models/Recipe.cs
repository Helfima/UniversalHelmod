using Calculator.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Databases.Models
{
    public class Recipe : Element
    {
        public Database Database { get; set; }
        public string DisplayName { get; set; }
        public List<string> MadeIn { get; set; }
        public double Energy { get; set; }
        public Builder Builder { get; set; }
        public bool Alternate { get; set; } = false;
        public RecipeCost RecipeCost { get; set; }
        public Item MainProduct => products.FirstOrDefault()?.Item;
        public string ItemType => products.FirstOrDefault()?.Item.ItemType;

    }
}
