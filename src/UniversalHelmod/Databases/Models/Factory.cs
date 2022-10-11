using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Databases.Models
{
    public class Factory : BaseOnItem
    {
        protected string type = "Factory";
        public string Type
        {
            get { return this.type; }
            set { this.type = value; NotifyPropertyChanged(); }
        }
        protected double speed;
        public double Speed
        {
            get { return this.speed; }
            set { this.speed = value; NotifyPropertyChanged(); }
        }
        protected double powerConsumption;
        public double PowerConsumption
        {
            get { return this.powerConsumption; }
            set { this.powerConsumption = value; NotifyPropertyChanged(); }
        }
        protected double powerProduction;
        public double PowerProduction
        {
            get { return this.powerProduction; }
            set { this.powerProduction = value; NotifyPropertyChanged(); }
        }
        public List<string> AllowedResourceForms { get; set; }
        public List<string> AllowedResources { get; set; }
        public Factory Clone()
        {
            var factory = new Factory()
            {
                Item = this.Item,
                Speed = this.Speed,
                PowerProduction = this.PowerProduction,
                PowerConsumption = this.PowerConsumption,
            };
            return factory;
        }
    }
}
