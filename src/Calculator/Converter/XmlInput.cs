using Calculator.Math;
using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Calculator.Converter
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

        public static XmlInput Parse(Amount amount)
        {
            XmlInput xmlNode = new XmlInput();
            xmlNode.Type = amount.Type;
            xmlNode.Name = amount.Name;
            xmlNode.Value = amount.Count;
            return xmlNode;
        }

        public Amount GetObject()
        {
            var item = Database.Intance.SelectItem(Name, Type);
            return new Amount(item, Value);
        }
    }
}
