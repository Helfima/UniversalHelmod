using Calculator.Databases.Models;
using Calculator.Sheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Calculator.Sheets.Converter
{
    [Serializable]
    [XmlRoot("Node")]
    public class XmlNode
    {
        [XmlAttribute("Id")]
        public int Id;

        [XmlAttribute("Type")]
        public string Type;

        [XmlAttribute("Name")]
        public string Name;

        [XmlAttribute("Factory")]
        public string Factory;

        [XmlAttribute("IsNodes")]
        public bool IsNodes;

        [XmlElement("Inputs")]
        public List<XmlInput> Inputs = new List<XmlInput>();

        [XmlElement("Children")]
        public List<XmlNode> Children = new List<XmlNode>();

    }
}
