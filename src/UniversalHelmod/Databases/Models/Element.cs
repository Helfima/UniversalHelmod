using UniversalHelmod.Classes;
using UniversalHelmod.Sheets.Math;
using UniversalHelmod.Sheets.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media.Imaging;

namespace UniversalHelmod.Databases.Models
{
    public abstract class Element : BaseElement
    {
        protected ObservableCollection<Amount> products = new ObservableCollection<Amount>();
        protected ObservableCollection<Amount> ingredients = new ObservableCollection<Amount>();
        
        public Nodes Parent { get; set; }
        public int Id { get; set; }
        protected bool invert = false;
        public bool Invert
        {
            get { return invert; }
            set { invert = value; NotifyPropertyChanged(); }
        }
        protected int index;
        public int Index
        {
            get { return index; }
            set { index = value; NotifyPropertyChanged(); }
        }
        protected string type;
        public string Type
        {
            get { return type; }
            set { type = value; NotifyPropertyChanged(); }
        }
        protected double count;
        public double Count
        {
            get { return count; }
            set { count = value; NotifyPropertyChanged(); }
        }
        protected double power;
        public double Power
        {
            get { return power; }
            set { power = value; NotifyPropertyChanged(); }
        }
        protected double production = 1;
        public double Production
        {
            get { return production; }
            set { production = value; NotifyPropertyChanged(); }
        }
        
        public ObservableCollection<Amount> Products { 
            get { return invert ? ingredients : products; } 
            set { products = value; NotifyPropertyChanged();}
        }
        public ObservableCollection<Amount> Ingredients
        {
            get { return invert ? products : ingredients; }
            set { ingredients = value; NotifyPropertyChanged(); }
        }

        public bool Match(MatrixValue other)
        {
            if (other == null || Type == null || Name == null) return false;
            return Type.Equals(other.Type) && Name.Equals(other.Name);
        }

        /// <summary>
        /// Compute flow
        /// </summary>
        /// <param name="duration">in second, default 60, flow per minute</param>
        public void ComputeFlow(int duration = 60, double energy=1)
        {
            var factor = duration / energy;
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
