using Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Models
{
    public abstract class BaseItem
    {
        protected string name;
        protected string displayName;
        protected string description;
        protected ItemType itemType = ItemType.Item;
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
        public ItemType ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }
        public BitmapImage Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public bool Match(Item other)
        {
            if (other == null || this.name == null) return false;
            return this.name.Equals(other.Name);
        }
    }
}
