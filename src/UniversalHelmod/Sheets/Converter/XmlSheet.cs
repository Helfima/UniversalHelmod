using UniversalHelmod.Databases.Models;
using UniversalHelmod.Sheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UniversalHelmod.Sheets.Converter
{
    [Serializable]
    [XmlRoot("Sheet")]
    public class XmlSheet
    {
        [XmlAttribute("Time")]
        public int Time;

        [XmlElement("Inputs")]
        public List<XmlInput> Inputs = new List<XmlInput>();

        [XmlElement("Children")]
        public List<XmlNode> Children = new List<XmlNode>();
    }
}
