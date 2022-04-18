using Calculator.Classes;
using Calculator.Converter;
using Calculator.Models;
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
        public void Load()
        {
            string path = Path.Combine(pathFolder, "database.json");
            Database = DatabaseConverter.ReadJson(path);
        }
    }
}
