using UniversalHelmod.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media.Imaging;

namespace UniversalHelmod.Databases.Models
{
    public class Builder : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Factory factory;
        private double count;
        private int powerShard;
        private Builder()
        {

        }
        public Builder(Factory factory)
        {
            this.factory = factory;
        }
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
            get { return factory.Icon; }
        }
        public double Speed
        {
            get { return factory.Speed; }
        }

        public double Power
        {
            get { return factory.PowerConsumption; }
        }
        public double Count
        {
            get { return count; }
            set { count = value; NotifyPropertyChanged(); }
        }
        public int PowerShard
        {
            get { return powerShard; }
            set { powerShard = value; NotifyPropertyChanged(); }
        }
        public string Name
        {
            get { return factory.Name; }
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
