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

        public static XmlNode Parse(Element element)
        {
            XmlNode xmlNode = new XmlNode();
            xmlNode.Id = element.Id;
            xmlNode.Name = element.Name;
            xmlNode.Type = element.Type;
            if(element is Node)
            {
                Node recipe = element as Node;
                xmlNode.Factory = recipe.Builder?.Name;
            }
            xmlNode.IsNodes = element is Nodes;
            if (element is Nodes)
            {
                Nodes nodes = (Nodes)element;
                if (nodes.Children != null)
                {
                    foreach (Element childNode in nodes.Children)
                    {
                        xmlNode.Children.Add(XmlNode.Parse(childNode));
                    }
                }
                if (nodes.Inputs != null)
                {
                    foreach (Amount input in nodes.Inputs)
                    {
                        xmlNode.Inputs.Add(XmlInput.Parse(input));
                    }
                }
            }
            return xmlNode;
        }

        public Element GetObject()
        {
            try
            {
                if (IsNodes)
                {
                    Nodes nodes = new Nodes();
                    if (Inputs != null)
                    {
                        foreach (XmlInput xmlInput in Inputs)
                        {
                            nodes.SetInput(xmlInput.GetObject());
                        }
                    }
                    foreach (XmlNode xmlNode in Children)
                    {
                        nodes.Add(xmlNode.GetObject());
                    }
                    return nodes;
                }
                else
                {
                    Node node = null;
                    switch (Type)
                    {
                        case "Recipe":
                            var recipe = Database.Intance.SelectRecipe(Name);
                            node = new Node(recipe);
                            node.Initialize();
                            break;
                    }
                    if (node != null && !String.IsNullOrEmpty(Factory))
                    {
                        Factory factory = Database.Intance.SelectFactory(Factory);
                        if (factory != null)
                        {
                            node.Builder = new Builder(factory);
                        }
                    }
                    return node as Element;
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return null;
        }
        
    }
}
