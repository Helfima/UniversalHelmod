using Calculator.Models;
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
    [XmlRoot("Sheet")]
    public class XmlSheet
    {
        [XmlAttribute("Time")]
        public int Time;

        [XmlElement("Inputs")]
        public List<XmlInput> Inputs = new List<XmlInput>();

        [XmlElement("Children")]
        public List<XmlNode> Children = new List<XmlNode>();

        public static XmlSheet Parse(Nodes nodes)
        {
            XmlSheet xmlNode = new XmlSheet();
            xmlNode.Time = nodes.Time;
            if (nodes.Inputs != null)
            {
                foreach (Amount input in nodes.Inputs)
                {
                    xmlNode.Inputs.Add(XmlInput.Parse(input));
                }
            }
            if (nodes.Children != null)
            {
                foreach (Element childNode in nodes.Children)
                {
                    xmlNode.Children.Add(XmlNode.Parse(childNode));
                }
            }
            return xmlNode;
        }

        public Nodes GetObject()
        {
            Nodes nodes = new Nodes(Time);
            if (Inputs != null)
            {
                foreach (XmlInput xmlInput in Inputs)
                {
                    nodes.SetInput(xmlInput.GetObject());
                }
            }
            if (Children != null)
            {
                foreach (XmlNode xmlNode in Children)
                {
                    nodes.Add(xmlNode.GetObject());
                }
            }
            return nodes;
        }
    }
}
