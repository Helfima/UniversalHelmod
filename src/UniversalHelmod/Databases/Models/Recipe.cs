using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UniversalHelmod.Enums;
using UniversalHelmod.Extensions;

namespace UniversalHelmod.Databases.Models
{
    public class Recipe : Element
    {
        public Database Database { get; set; }
        protected string displayName;
        public string DisplayName
        {
            get { return this.displayName; }
            set { this.displayName = value; NotifyPropertyChanged(); }
        }
        protected string description;
        public string Description
        {
            get { return this.description; }
            set { this.description = value; NotifyPropertyChanged(); }
        }
        protected int tier;
        public int Tier
        {
            get { return this.tier; }
            set { this.tier = value; NotifyPropertyChanged(); }
        }
        protected double energy;
        public double Energy
        {
            get { return this.energy; }
            set { this.energy = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<string> madeIn = new ObservableCollection<string>();
        public ObservableCollection<string> MadeIn
        {
            get { return this.madeIn; }
            set { this.madeIn = value; NotifyPropertyChanged(); }
        }
        public Builder Builder { get; set; }
        public RecipeCost RecipeCost { get; set; }
        public Item MainProduct => products.FirstOrDefault()?.Item;
        public string ItemType => products.FirstOrDefault()?.Item.Type;
        public Recipe Clone()
        {
            var recipe = new Recipe()
            {
                Database = this.Database,
                Name = this.Name,
                DisplayName = this.DisplayName,
                Description = this.Description,
                IconPath = this.IconPath,
                Overlay = this.Overlay,
                MadeIn = this.MadeIn,
                Energy = this.Energy,
                Tier = this.Tier,
                Products = this.Products.Select(x => x.Clone()).ToObservableCollection(),
                Ingredients = this.Ingredients.Select(x => x.Clone()).ToObservableCollection()
            };
            return recipe;
        }
    }
}
