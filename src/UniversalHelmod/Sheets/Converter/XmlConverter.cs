using UniversalHelmod.Databases.Models;
using UniversalHelmod.Sheets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Sheets.Converter
{
    class XmlConverter
    {
        #region ======== Format ========
        public static XmlModel Format(DataModel dataModel)
        {
            XmlModel xmlModel = new XmlModel();
            xmlModel.Version = dataModel.Version;
            if (dataModel.Sheets != null)
            {
                foreach (Nodes nodes in dataModel.Sheets)
                {
                    var xmlNode = Format(nodes);
                    xmlModel.Sheets.Add(xmlNode);
                }
            }
            return xmlModel;
        }
        internal static XmlSheet Format(Nodes nodes)
        {
            XmlSheet xmlNode = new XmlSheet();
            xmlNode.Time = nodes.Time;
            if (nodes.Inputs != null)
            {
                foreach (Amount input in nodes.Inputs)
                {
                    var xmlInput = Format(input);
                    xmlNode.Inputs.Add(xmlInput);
                }
            }
            if (nodes.Children != null)
            {
                foreach (Element childNode in nodes.Children)
                {
                    var xmlChild = Format(childNode);
                    xmlNode.Children.Add(xmlChild);
                }
            }
            return xmlNode;
        }
        internal static XmlNode Format(Element element)
        {
            XmlNode xmlNode = new XmlNode();
            xmlNode.Id = element.Id;
            xmlNode.Name = element.Name;
            xmlNode.Type = element.Type;
            if (element is Node)
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
                        var xmlChild = Format(childNode);
                        xmlNode.Children.Add(xmlChild);
                    }
                }
                if (nodes.Inputs != null)
                {
                    foreach (Amount input in nodes.Inputs)
                    {
                        var xmlInput = Format(input);
                        xmlNode.Inputs.Add(xmlInput);
                    }
                }
            }
            return xmlNode;
        }
        internal static XmlInput Format(Amount amount)
        {
            XmlInput xmlNode = new XmlInput();
            xmlNode.Type = amount.Type;
            xmlNode.Name = amount.Name;
            xmlNode.Value = amount.Count;
            return xmlNode;
        }
        #endregion

        #region ======== Parse ========
        public static DataModel Parse(XmlModel xmlModel, Database database)
        {
            DataModel dataModel = new DataModel(database, xmlModel.Version);
            if (xmlModel.Sheets != null)
            {
                //Classes.HMLogger.Debug($"Xml sheets: {Sheets.Count}");
                foreach (XmlSheet xmlSheet in xmlModel.Sheets)
                {
                    var nodes = Parse(xmlSheet, database);
                    dataModel.Sheets.Add(nodes);
                }
            }
            return dataModel;
        }
        internal static Nodes Parse(XmlSheet xmlSheet, Database database)
        {
            Nodes nodes = new Nodes(database, xmlSheet.Time);
            if (xmlSheet.Inputs != null)
            {
                foreach (XmlInput xmlInput in xmlSheet.Inputs)
                {
                    var input = Parse(xmlInput, database);
                    nodes.SetInput(input);
                }
            }
            if (xmlSheet.Children != null)
            {
                foreach (XmlNode xmlChild in xmlSheet.Children)
                {
                    var child = Parse(xmlChild, database);
                    nodes.Add(child);
                }
            }
            return nodes;
        }
        internal static Element Parse(XmlNode xmlNode, Database database)
        {
            try
            {
                if (xmlNode.IsNodes)
                {
                    Nodes nodes = new Nodes(database);
                    if (xmlNode.Inputs != null)
                    {
                        foreach (XmlInput xmlInput in xmlNode.Inputs)
                        {
                            var input = Parse(xmlInput, database);
                            nodes.SetInput(input);
                        }
                    }
                    foreach (XmlNode xmlChild in xmlNode.Children)
                    {
                        var child = Parse(xmlChild, database);
                        nodes.Add(child);
                    }
                    return nodes;
                }
                else
                {
                    Node node = null;
                    switch (xmlNode.Type)
                    {
                        default:
                            var recipe = database.SelectRecipe(xmlNode.Name);
                            node = new Node(recipe);
                            node.Initialize();
                            break;
                    }
                    if (node != null && !String.IsNullOrEmpty(xmlNode.Factory))
                    {
                        Factory factory = database.SelectFactory(xmlNode.Factory);
                        if (factory != null)
                        {
                            node.Builder = new Builder(factory);
                        }
                    }
                    return node as Element;
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return null;
        }
        internal static Amount Parse(XmlInput xmlInput, Database database)
        {
            var item = database.SelectItem(xmlInput.Name, xmlInput.Type);
            return new Amount(item, xmlInput.Value);
        }
        #endregion

    }
}
