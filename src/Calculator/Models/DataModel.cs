using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Calculator.Models
{
    public class DataModel :  NotifyProperty
    {
        private int version = 1;
        private ObservableCollection<Nodes> sheets;
        private ObservableCollection<Nodes> flatNodes;
        private Nodes currentSheet;
        private Nodes currentNode;

        public DataModel(int version = 1)
        {
            this.version = version;
            Load();
        }
        public int Version
        {
            get { return version; }
        }
        public ObservableCollection<Nodes> Sheets
        {
            get { return sheets; }
        }
        public Nodes CurrentSheet
        {
            get { return currentSheet; }
            set { currentSheet = value; NotifyPropertyChanged(); }
        }
        public Nodes CurrentNode
        {
            get { return currentNode; }
            set { currentNode = value; NotifyPropertyChanged(); }
        }
        public ObservableCollection<Nodes> FlatNodes
        {
            get { return flatNodes; }
            set { flatNodes = value; NotifyPropertyChanged(); }
        }
        private void Load()
        {
            sheets = new ObservableCollection<Nodes>();
            currentSheet = Sheets.FirstOrDefault();
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
