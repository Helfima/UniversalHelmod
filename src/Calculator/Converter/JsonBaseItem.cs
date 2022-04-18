using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Converter
{
    public abstract class JsonBaseItem
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
    }
}
