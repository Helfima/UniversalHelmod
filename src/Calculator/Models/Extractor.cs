using Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Models
{
    public class Extractor : Factory
    {
        public List<string> AllowedResourceForms { get; set; }
        public List<string> AllowedResources { get; set; }
    }
}
