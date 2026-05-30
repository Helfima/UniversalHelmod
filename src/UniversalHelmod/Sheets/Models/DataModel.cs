using UniversalHelmod.Classes;
using UniversalHelmod.Databases.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UniversalHelmod.Extensions;

namespace UniversalHelmod.Sheets.Models
{
    public class DataModel : NotifyProperty
    {
        private Database database;
        private int version = 1;
        
        public DataModel(Database database, int version = 1)
        {
            this.database = database;
            this.version = version;
        }
        public int Version
        {
            get { return version; }
        }
        private ObservableCollection<Nodes> sheets = new ObservableCollection<Nodes>();
        public ObservableCollection<Nodes> Sheets
        {
            get { return sheets; }
        }
        private Nodes currentSheet;
        public Nodes CurrentSheet
        {
            get { return currentSheet; }
            set { currentSheet = value; NotifyPropertyChanged(); }
        }
        private Nodes currentNode;
        public Nodes CurrentNode
        {
            get { return currentNode; }
            set { currentNode = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<Nodes> flatNodes = new ObservableCollection<Nodes>();
        public ObservableCollection<Nodes> FlatNodes
        {
            get { return flatNodes; }
            set { flatNodes = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<LogisticForm> logisticForms = new ObservableCollection<LogisticForm>();
        public ObservableCollection<LogisticForm> LogisticForms
        {
            get { return logisticForms; }
            set { logisticForms = value; NotifyPropertyChanged(); }
        }
        public void UpdateLogisticForms()
        {
            foreach(var form in this.database.ItemForms)
            {
                var logisticForm = new LogisticForm()
                {
                    Name = form,
                    Items = this.database.Logistics.Where(x => x.Form == form).ToObservableCollection()
                };
                if(logisticForm.Items.Count > 0)
                {
                    logisticForm.SelectedItem = logisticForm.Items.MaxBy(x => x.Flow);
                    this.LogisticForms.Add(logisticForm);
                }
            }
        }
        public void UpdateFlatNodes()
        {
            var flatNodes = new ObservableCollection<Nodes>();
            if (currentSheet != null)
            {
                GetFlatNodes(CurrentSheet, ref flatNodes, 0);
            }
            FlatNodes = flatNodes;
        }
        private void GetFlatNodes(Nodes nodes, ref ObservableCollection<Nodes> flatNodes, double offset)
        {
            nodes.Offset = offset;
            flatNodes.Add(nodes);
            foreach (var node in nodes.Children)
            {
                if (node is Nodes)
                {
                    GetFlatNodes((Nodes)node, ref flatNodes, offset + 20);
                }
            }
        }
    }
}
