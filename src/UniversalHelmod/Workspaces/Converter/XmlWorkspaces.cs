using UniversalHelmod.Workspaces.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace UniversalHelmod.Workspaces.Converter
{
    [Serializable]
    [XmlRoot("Workspaces")]
    public class XmlWorkspaces
    {
        [XmlElement("Workspace")]
        public List<XmlWorkspace> Workspaces { get; set; } = new List<XmlWorkspace>();

        public static XmlWorkspaces Parse(WorkspacesModel model)
        {
            XmlWorkspaces xmlModel = new XmlWorkspaces();
            if (model.Workspaces != null)
            {
                foreach (Workspace workspace in model.Workspaces)
                {
                    xmlModel.Workspaces.Add(XmlWorkspace.Parse(workspace));
                }
            }
            return xmlModel;
        }

        public WorkspacesModel GetObject()
        {
            WorkspacesModel dataModel = new WorkspacesModel();
            if (Workspaces != null)
            {
                //Classes.HMLogger.Debug($"Xml sheets: {Sheets.Count}");
                foreach (XmlWorkspace xmlWorkspace in Workspaces)
                {
                    dataModel.Workspaces.Add(xmlWorkspace.GetObject());
                }
            }
            return dataModel;
        }
    }
}
