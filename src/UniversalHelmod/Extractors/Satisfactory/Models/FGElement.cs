using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using UniversalHelmod.Extensions;

namespace UniversalHelmod.Extractors.Satisfactory.Models
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
