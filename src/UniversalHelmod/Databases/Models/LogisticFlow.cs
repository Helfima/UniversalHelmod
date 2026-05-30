using System;
using System.Collections.Generic;
using System.Text;
using UniversalHelmod.Classes;

namespace UniversalHelmod.Databases.Models
{
    public class LogisticFlow : NotifyProperty
    {
        private Logistic item;
        public Logistic Item
        {
            get { return item; }
            set { item = value; NotifyPropertyChanged(); }
        }
        protected double flow;
        public double Flow
        {
            get { return this.flow; }
            set { this.flow = value; NotifyPropertyChanged(); }
        }
    }
}
