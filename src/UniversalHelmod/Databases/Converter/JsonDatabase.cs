using UniversalHelmod.Databases.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Databases.Converter
{
    public class JsonDatabase
    {
        public List<JsonItem> Items { get; set; } = new List<JsonItem>();
        public List<JsonFactory> Factories { get; set; } = new List<JsonFactory>();
        public List<JsonLogistic> Logistics { get; set; } = new List<JsonLogistic>();
        public List<JsonRecipe> Recipes { get; set; } = new List<JsonRecipe>();
    }
}
