using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Databases.Models
{
    public class Factory : BaseItem
    {
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
                Name = this.Name,
                DisplayName = this.DisplayName,
                Description = this.Description,
                Type = this.Type,
                Speed = this.Speed,
                PowerProduction = this.PowerProduction,
                PowerConsumption = this.PowerConsumption,
            };
            return factory;
        }
    }
}
