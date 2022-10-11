using UniversalHelmod.Databases.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Databases.Converter
{
    public abstract class JsonBaseItem
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public List<Property> Properties { get; set; }
    }
}
