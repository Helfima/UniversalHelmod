using Calculator.Classes;
using Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Databases.Models
{
    public abstract class BaseItem
    {
        public Database Database { get; set; }
        protected string name;
        protected string displayName;
        protected string description;
        protected string itemType = "Item";
        protected string iconPath;
        protected BitmapImage icon;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }
        public string IconPath
        {
            get { return iconPath; }
            set { iconPath = value; }
        }
        public BitmapImage Icon
        {
            get {
                if (icon == null) icon = Utils.GetImage(IconPath);
                return icon;
            }
            set { icon = value; }
        }

        public bool Match(Item other)
        {
            if (other == null || this.name == null) return false;
            return this.name.Equals(other.Name);
        }
    }
}
