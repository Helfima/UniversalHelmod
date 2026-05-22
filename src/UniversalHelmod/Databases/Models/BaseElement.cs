using UniversalHelmod.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace UniversalHelmod.Databases.Models
{
    public abstract class BaseElement : BaseIcon
    {
        protected string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; NotifyPropertyChanged(); }
        }
        protected string tag;
        public string Tag
        {
            get { return this.tag; }
            set { this.tag = value; NotifyPropertyChanged(); }
        }
        public bool Match(BaseElement other)
        {
            if (other == null || this.name == null) return false;
            return this.name.Equals(other.Name);
        }
    }
}
