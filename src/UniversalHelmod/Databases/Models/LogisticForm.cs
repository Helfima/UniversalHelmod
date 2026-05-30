using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UniversalHelmod.Classes;
using UniversalHelmod.Sheets.Models;

namespace UniversalHelmod.Databases.Models
{
    public class LogisticForm : NotifyProperty
    {
        protected string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<Logistic> items;
        public ObservableCollection<Logistic> Items
        {
            get { return items; }
            set { items = value; NotifyPropertyChanged(); }
        }
        protected Logistic selectedItem;
        public Logistic SelectedItem
        {
            get { return this.selectedItem; }
            set { this.selectedItem = value; NotifyPropertyChanged(); }
        }
    }
}
