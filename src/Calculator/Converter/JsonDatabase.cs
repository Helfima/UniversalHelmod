using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Converter
{
    public class JsonDatabase
    {
        public List<JsonItem> Items { get; set; } = new List<JsonItem>();
        public List<JsonFactory> Factories { get; set; } = new List<JsonFactory>();
        public List<JsonRecipe> Recipes { get; set; } = new List<JsonRecipe>();
    }
}
