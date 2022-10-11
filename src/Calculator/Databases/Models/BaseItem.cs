using Calculator.Classes;
using Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Databases.Models
{
    public abstract class BaseItem : BaseElement
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
        protected string type = "Item";
        public string Type
        {
            get { return this.type; }
            set { this.type = value; NotifyPropertyChanged(); }
        }
        protected ObservableCollection<Property> properties = new ObservableCollection<Property>();
        public ObservableCollection<Property> Properties
        {
            get { return this.properties; }
            set { this.properties = value; NotifyPropertyChanged(); }
        }
    }
}
