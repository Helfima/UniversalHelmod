using UniversalHelmod.Enums;
using UniversalHelmod.Extractors;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;

namespace UniversalHelmod.Databases.Models
{
    public class Item : BaseItem
    {
        protected int stackSize;
        public int StackSize
        {
            get { return this.stackSize; }
            set { this.stackSize = value; NotifyPropertyChanged(); }
        }
        protected double energyValue;
        public double EnergyValue
        {
            get { return this.energyValue; }
            set { this.energyValue = value; NotifyPropertyChanged(); }
        }
        protected double radioactiveDecay;
        public double RadioactiveDecay
        {
            get { return this.radioactiveDecay; }
            set { this.radioactiveDecay = value; NotifyPropertyChanged(); }
        }
        protected int resourceSinkPoints;
        public int ResourceSinkPoints
        {
            get { return this.resourceSinkPoints; }
            set { this.resourceSinkPoints = value; NotifyPropertyChanged(); }
        }
        protected string form;
        public string Form
        {
            get { return this.form; }
            set { this.form = value; NotifyPropertyChanged(); }
        }
        public ItemCost ItemCost { get; set; }
        public List<Item> WhereUsed { get; set; } = new List<Item>();

        public Item Clone()
        {
            var item = new Item()
            {
                Name = this.Name,
                DisplayName = this.DisplayName,
                Description = this.Description,
                Type = this.Type,
                IconPath = this.IconPath,
                StackSize = this.StackSize,
                EnergyValue = this.EnergyValue,
                Form = this.Form,
            };
            return item;
        }
    }
}
