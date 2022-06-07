using Calculator.Classes;
using Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Databases.Models
{
    public abstract class BaseItem : NotifyProperty
    {
        public Database Database { get; set; }

        protected string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; NotifyPropertyChanged(); }
        }
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
        protected string itemType = "Item";
        public string ItemType
        {
            get { return this.itemType; }
            set { this.itemType = value; NotifyPropertyChanged(); }
        }
        protected string iconPath;
        public string IconPath
        {
            get { return this.iconPath; }
            set { this.iconPath = value; NotifyPropertyChanged(); }
        }
        protected BitmapImage icon;
        public BitmapImage Icon
        {
            get {
                if (icon == null) this.icon = Utils.GetImage(IconPath);
                return this.icon;
            }
            set { this.icon = value; NotifyPropertyChanged(); }
        }

        public bool Match(Item other)
        {
            if (other == null || this.name == null) return false;
            return this.name.Equals(other.Name);
        }
    }
}
