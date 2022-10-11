using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Databases.Models
{
    public class ItemCost
    {
        private Item item;
        private Dictionary<Item, double> cost = new Dictionary<Item, double>();

        public ItemCost(Item item)
        {
            this.item = item;
        }
        public Dictionary<Item, double> Cost
        {
            get { return cost; }
        }

        public Item Item
        {
            get { return item; }
        }

        public void Add(Item item, double value)
        {
            if (!cost.ContainsKey(item)) cost.Add(item, 0);
            cost[item] += value;
        }
        public void Add(ItemCost itemCost, double factor)
        {
            foreach(KeyValuePair<Item, double> keyValue in itemCost.Cost)
            {
                Add(keyValue.Key, keyValue.Value * factor);
            }
        }
    }
}
