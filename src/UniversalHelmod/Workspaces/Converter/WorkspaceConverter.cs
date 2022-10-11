using UniversalHelmod.Classes;
using UniversalHelmod.Workspaces.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace UniversalHelmod.Workspaces.Converter
{
    class WorkspaceConverter
    {
        public static string GetFilename()
        {
            string root = Utils.GetApplicationFolder();
            return Path.Combine(root, "workspacesmodel.xml");
        }
        public static void WriteXml(WorkspacesModel model)
        {
            string path = GetFilename();
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (Directory.Exists(directory))
            {
                SerializeItem(path, model);
            }
            else
            {
                System.Console.WriteLine($"Unable to write file: {path}");
            }
        }
        public static WorkspacesModel ReadXml()
        {
            string path = GetFilename();
            WorkspacesModel model = DeserializeItem(path);
            if (model == null)
            {
                model = new WorkspacesModel();
            }
            return model;
        }

        internal static void SerializeItem(string fileName, WorkspacesModel model)
        {
            // Create an instance of the type and serialize it.
            XmlSerializer serializer = new XmlSerializer(typeof(XmlWorkspaces));
            serializer.UnknownNode += new XmlNodeEventHandler(Serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(Serializer_UnknownAttribute);

            XmlWorkspaces xmlModel = XmlWorkspaces.Parse(model);
            FileStream writer = new FileStream(fileName, FileMode.Create);
            serializer.Serialize(writer, xmlModel);
            writer.Close();
        }

        internal static WorkspacesModel DeserializeItem(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileStream reader = new FileStream(fileName, FileMode.Open);
                XmlSerializer deserializer = new XmlSerializer(typeof(XmlWorkspaces));
                XmlWorkspaces xmlModel = (XmlWorkspaces)deserializer.Deserialize(reader);
                reader.Close();
                return xmlModel.GetObject();
            }
            return null;
        }

        internal static void Serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            throw new Exception($"CustomProperties: Unknown Node:{e.Name}\t{e.Text}");
        }

        internal static void Serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            throw new Exception($"CustomProperties: Unknown attribute:{attr.Name}='{attr.Value}'");
        }
    }
}
