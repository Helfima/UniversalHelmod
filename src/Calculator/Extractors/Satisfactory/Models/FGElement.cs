using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Calculator.Extensions;

namespace Calculator.Extractors.Satisfactory.Models
{
    public class FGElement
    {
        public string ClassName;
        public FGElement() { }
        public FGElement(JsonElement element)
        {
            ClassName = element.GetStringValue("ClassName");
        }
    }
}
