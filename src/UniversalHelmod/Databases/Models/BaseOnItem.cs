using UniversalHelmod.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UniversalHelmod.Databases.Models
{
    public class BaseOnItem : NotifyProperty
    {
        private Item item;
        public Item Item
        {
            get { return this.item; }
            set { this.item = value; NotifyPropertyChanged(); }
        }
        protected ObservableCollection<Property> properties = new ObservableCollection<Property>();
        public ObservableCollection<Property> Properties
        {
            get { return this.properties; }
            set { this.properties = value; NotifyPropertyChanged(); }
        }
    }
}
