using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media.Imaging;
using UniversalHelmod.Classes;
using UniversalHelmod.Enums;

namespace UniversalHelmod.Databases.Models
{
    public class Builder : NotifyProperty
    {
        public Builder()
        {

        }
        public Builder(Factory factory)
        {
            this.factory = factory;
        }
        private Factory factory;
        public Factory Factory
        {
            get { return factory; }
        }
        public string Type
        {
            get { return factory.Type; }
        }
        public BitmapImage Icon
        {
            get { return factory?.Icon; }
        }
        public double Speed
        {
            get { return factory ==null ? 1 : factory.Speed; }
        }

        public double Power
        {
            get { return factory == null ? 0 : factory.PowerConsumption; }
        }
        private double count;
        public double Count
        {
            get { return count; }
            set { count = value; NotifyPropertyChanged(); }
        }
        private int powerShard;
        public int PowerShard
        {
            get { return powerShard; }
            set { powerShard = value; NotifyPropertyChanged(); }
        }
        public string Name
        {
            get { return factory?.Name; }
        }
        public Builder Clone(double factor = 1)
        {
            var item = new Builder()
            {
                factory = this.factory,
                count = this.count * factor,
            };
            return item;
        }
    }
}
