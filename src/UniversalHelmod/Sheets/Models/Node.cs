using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UniversalHelmod.Databases.Models;

namespace UniversalHelmod.Sheets.Models
{
    public class Node : Element
    {
        private Recipe recipe;
        protected Effects effects = new Effects();

        public Builder Builder { get; set; }
        public Effects Effects => effects;
        public Recipe Recipe => recipe;

        public Node(Recipe recipe)
        {
            this.recipe = recipe;
            this.name = recipe.Name;
            this.type = recipe.Type;
            this.icon = recipe.Icon;
            var factory = recipe.Database.Factories.FirstOrDefault(x => recipe.MadeIn.Contains(x.Item.Name));
            this.Builder = new Builder(factory);
        }
        public void UpdateEffect()
        {
            double overclock = 1 + Builder.PowerShard * 0.5;
            var powerConsumptionExponent = Builder.Factory.Properties.Where(x => x.Name == "PowerConsumptionExponent").FirstOrDefault();
            double consumption = 1;
            if (powerConsumptionExponent != null)
            {
                var value = System.Convert.ToDouble(powerConsumptionExponent.Value);
                consumption = System.Math.Pow(overclock, value);
            }
            effects.Speed = overclock;
            effects.Consumption = consumption;
        }
        public void Initialize()
        {
            this.ingredients.Clear();
            foreach (Amount amount in this.Recipe.Ingredients)
            {
                this.ingredients.Add(amount.Clone());
            }
            this.products.Clear();
            foreach (Amount amount in this.Recipe.Products)
            {
                this.products.Add(amount.Clone());
            }
        }
        /// <summary>
        /// Compute flow
        /// </summary>
        /// <param name="duration">in second, default 60, flow per minute</param>
        public void ComputeFlow(int duration = 60)
        {
            var factor = duration / this.Recipe.Energy;
            foreach (Amount amount in this.Ingredients)
            {
                amount.Flow = amount.Count * factor;
            }
            foreach (Amount amount in this.Products)
            {
                amount.Flow = amount.Count * factor;
            }
        }
    }
}
