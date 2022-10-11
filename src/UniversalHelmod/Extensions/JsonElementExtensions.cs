using UniversalHelmod.Extractors.Satisfactory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace UniversalHelmod.Extensions
{
    public static class JsonElementExtensions
    {
        public static string GetStringValue(this JsonElement element, string name)
        {
            try
            {
                return element.GetProperty(name).GetString();
            }
            catch
            {
                return "";
            }
        }

        public static double GetDoubleValue(this JsonElement element, string name)
        {
            try
            {
                var property = element.GetProperty(name).GetString();
                double value;
                Double.TryParse(property, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out value);
                return value;
            }
            catch
            {
                return 0;
            }
        }
        public static bool GetBooleanValue(this JsonElement element, string name)
        {
            var property = element.GetProperty(name).GetString();
            if (property == "True") return true;
            return false;
        }
        public static int GetIntegerValue(this JsonElement element, string name)
        {
            try
            {
                var property = element.GetProperty(name).GetString();
                int value;
                Int32.TryParse(property, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out value);
                return value;
            }
            catch
            {
                return 0;
            }
        }

        public static object GetArrayValue(this JsonElement element, string name)
        {
            try
            {
                string property = element.GetProperty(name).GetString();
                object result = Extractors.Satisfactory.Models.FGArrayParser.Parse(property);
                return result;
            }
            catch
            {
                return "";
            }
        }
    }
}
