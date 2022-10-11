using Calculator.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Databases.Models
{
    public class Property : NotifyProperty
    {
        public Property() { }
        public Property(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
        protected string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; NotifyPropertyChanged(); }
        }
        protected object value;
        public object Value
        {
            get { return this.value; }
            set { this.value = value; NotifyPropertyChanged(); }
        }
    }
}
