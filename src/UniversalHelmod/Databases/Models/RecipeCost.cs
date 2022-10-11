using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Databases.Models
{
    public class RecipeCost
    {
        private Recipe recipe;
        private Dictionary<Item, double> cost = new Dictionary<Item, double>();

        public RecipeCost(Recipe recipe)
        {
            this.recipe = recipe;
        }
        public Dictionary<Item, double> Cost
        {
            get { return cost; }
        }

        public Recipe Recipe
        {
            get { return recipe; }
        }

        public void Add(Item item, double value)
        {
            if (!cost.ContainsKey(item)) cost.Add(item, 0);
            cost[item] += value;
        }
        public void Add(ItemCost itemCost, double factor)
        {
            foreach (KeyValuePair<Item, double> keyValue in itemCost.Cost)
            {
                Add(keyValue.Key, keyValue.Value * factor);
            }
        }
    }
}
