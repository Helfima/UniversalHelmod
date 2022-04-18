using Calculator.Workspaces.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Calculator.Workspaces.Converter
{
    [Serializable]
    [XmlRoot("Workspace")]
    public class XmlWorkspace
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public static XmlWorkspace Parse(Workspace model)
        {
            XmlWorkspace xmlModel = new XmlWorkspace();
            xmlModel.Name = model.Name;
            xmlModel.Path = model.PathFolder;
            xmlModel.CreatedDate = model.CreatedDate;
            xmlModel.ModifiedDate = model.ModifiedDate;
            return xmlModel;
        }

        public Workspace GetObject()
        {
            Workspace model = new Workspace();
            model.Name = this.Name;
            model.PathFolder = this.Path;
            model.CreatedDate = this.CreatedDate;
            model.ModifiedDate = this.ModifiedDate;
            return model;
        }
    }
}
