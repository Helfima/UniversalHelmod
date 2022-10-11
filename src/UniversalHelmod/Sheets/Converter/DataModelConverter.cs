﻿using UniversalHelmod.Databases.Models;
using UniversalHelmod.Sheets.Math;
using UniversalHelmod.Sheets.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;


namespace UniversalHelmod.Sheets.Converter
{
    public class DataModelConverter
    {
        public static void WriteXml(DataModel dataModel, string path)
        {
            if (path == null) throw new Exception("Path file error");
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (Directory.Exists(directory))
            {
                SerializeItem(path, dataModel);
            }
            else
            {
                System.Console.WriteLine($"Unable to write file: {path}");
            }
        }
        public static DataModel ReadXml(Database database, string path)
        {
            if (path == null) throw new Exception("Path file error");
            DataModel model = DeserializeItem(path, database);
            if (model != null)
            {
                foreach (Nodes sheet in model.Sheets)
                {
                    Compute compute = new Compute();
                    compute.Update(sheet);
                }
                var current = model.Sheets.FirstOrDefault();
                model.CurrentSheet = current;
                model.CurrentNode = current;
                return model;
            }
            return new DataModel(database);
        }

        internal static void SerializeItem(string fileName, DataModel dataModel)
        {
            // Create an instance of the type and serialize it.
            XmlSerializer serializer = new XmlSerializer(typeof(XmlModel));
            serializer.UnknownNode += new XmlNodeEventHandler(Serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(Serializer_UnknownAttribute);

            XmlModel xmlModel = XmlConverter.Format(dataModel);
            FileStream writer = new FileStream(fileName, FileMode.Create);
            serializer.Serialize(writer, xmlModel);
            writer.Close();
        }

        internal static DataModel DeserializeItem(string fileName, Database database)
        {
            if (File.Exists(fileName))
            {
                FileStream reader = new FileStream(fileName, FileMode.Open);
                XmlSerializer deserializer = new XmlSerializer(typeof(XmlModel));
                XmlModel xmlModel = (XmlModel)deserializer.Deserialize(reader);
                reader.Close();
                var dataModel = XmlConverter.Parse(xmlModel, database);
                return dataModel;
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
