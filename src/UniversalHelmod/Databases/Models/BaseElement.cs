using UniversalHelmod.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace UniversalHelmod.Databases.Models
{
    public abstract class BaseElement : NotifyProperty
    {
        protected string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; NotifyPropertyChanged(); }
        }
        protected string iconPath;
        public string IconPath
        {
            get { return iconPath; }
            set { iconPath = value; NotifyPropertyChanged(); }
        }
        protected BitmapImage icon;
        public BitmapImage Icon
        {
            get
            {
                if (icon == null) icon = Utils.GetImage(IconPath);
                return icon;
            }
            set { icon = value; NotifyPropertyChanged(); }
        }
        public bool Match(BaseElement other)
        {
            if (other == null || this.name == null) return false;
            return this.name.Equals(other.Name);
        }

    }
}
