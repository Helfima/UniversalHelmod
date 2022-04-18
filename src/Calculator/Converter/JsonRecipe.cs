using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Converter
{
    public class JsonRecipe : JsonBaseItem
    {
        public List<JsonAmount> Products { get; set; } = new List<JsonAmount>();
        public List<JsonAmount> Ingredients { get; set; } = new List<JsonAmount>();
        public List<string> MadeIn { get; set; }
        public double Energy { get; set; }
        public bool Alternate { get; set; } = false;
    }
}
