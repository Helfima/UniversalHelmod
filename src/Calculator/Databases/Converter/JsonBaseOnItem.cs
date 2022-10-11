using Calculator.Databases.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Databases.Converter
{
    public abstract class JsonBaseOnItem
    {
        public JsonElement BaseOnItem { get; set; }
        public List<Property> Properties { get; set; }
    }
}
