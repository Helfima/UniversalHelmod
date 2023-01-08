using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using UniversalHelmod.Classes;

namespace UniversalHelmod.Databases.Models
{
    public class BaseIcon : NotifyProperty
    {
        protected string iconPath;
        public string IconPath
        {
            get { return iconPath; }
            set { iconPath = value; Icon = null; NotifyPropertyChanged(); }
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
    }
}
