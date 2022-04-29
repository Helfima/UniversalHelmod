using Calculator.Databases.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Calculator.Sheets.Converter
{
    [Serializable]
    [XmlRoot("Input")]
    public class XmlInput
    {
        [XmlAttribute("Type")]
        public string Type;

        [XmlAttribute("Name")]
        public string Name;

        [XmlAttribute("Value")]
        public double Value;
    }
}
