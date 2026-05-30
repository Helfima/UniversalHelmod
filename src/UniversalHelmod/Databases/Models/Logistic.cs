using UniversalHelmod.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Databases.Models
{
    public class Logistic : Item
    {
        protected double flow;
        public double Flow
        {
            get { return this.flow; }
            set { this.flow = value; NotifyPropertyChanged(); }
        }
        public override Logistic Clone()
        {
            var item = new Logistic()
            {
                Database = this.Database,
                Name = this.Name,
                DisplayName = this.DisplayName,
                Description = this.Description,
                Type = this.Type,
                Form = this.Form,
                Tag = this.Tag,
                IconPath = this.IconPath,
                Overlay = this.Overlay,
                Flow = this.Flow,
                Properties = this.Properties
            };
            return item;
        }
    }
}
