using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Converter
{
    public class JsonItem : JsonBaseItem
    {
        public string Form { get; set; }
        public int StackSize { get; set; }
        public double EnergyValue { get; set; }
    }
}
