using Calculator.Classes;
using Calculator.Databases.Converter;
using Calculator.Databases.Models;
using Calculator.Sheets.Converter;
using Calculator.Sheets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calculator.Workspaces.Models
{
    public class Workspace : NotifyProperty
    {
        private Database database;
        public Database Database
        {
            get { return database; }
            set { database = value; NotifyPropertyChanged(); }
        }
        private DataModel dataModel;
        public DataModel DataModel
        {
            get { return dataModel; }
            set { dataModel = value; NotifyPropertyChanged(); }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged(); }
        }
        private string pathFolder;
        public string PathFolder
        {
            get { return pathFolder; }
            set { pathFolder = value; NotifyPropertyChanged(); }
        }
        private DateTime createdDate;
        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; NotifyPropertyChanged(); }
        }
        private DateTime modifiedDate;
        public DateTime ModifiedDate
        {
            get { return modifiedDate; }
            set { modifiedDate = value; NotifyPropertyChanged(); }
        }
        public void Initialize()
        {
            if (!Directory.Exists(pathFolder))
            {
                Directory.CreateDirectory(pathFolder);
            }
        }
        public void SaveDataModel()
        {
            string pathDataModel = Path.Combine(pathFolder, "data_model.xml");
            DataModelConverter.WriteXml(DataModel, pathDataModel);
        }
        public void SaveDatabase()
        {
            string pathDatabase = Path.Combine(pathFolder, "database.json");
            DatabaseConverter.WriteJson(WorkspacesModel.Intance.Current.Database, pathDatabase);
        }
        public void Load()
        {
            string pathDatabase = Path.Combine(pathFolder, "database.json");
            if (File.Exists(pathDatabase))
            {
                Database = DatabaseConverter.ReadJson(pathDatabase);
            }
            else
            {
                Database = new Database();
            }
            string pathDataModel = Path.Combine(pathFolder, "data_model.xml");
            if (File.Exists(pathDataModel))
            {
                DataModel = DataModelConverter.ReadXml(Database, pathDataModel);
            }
            else
            {
                DataModel = new DataModel(Database);
            }
        }
    }
}
