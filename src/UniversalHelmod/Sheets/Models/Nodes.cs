using UniversalHelmod.Extensions;
using UniversalHelmod.Databases.Models;
using UniversalHelmod.Sheets.Math;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace UniversalHelmod.Sheets.Models
{
    public class Nodes : Element
    {
        MatrixValue[] objectives;
        ObservableCollection<Amount> inputs = new ObservableCollection<Amount>();
        private int time;
        private double offset;
        private Database database;

        public Nodes(Database database, int time = 1)
        {
            this.database = database;
            this.time = 1;
        }
        public Database Database => database;
        public double Offset
        {
            get { return offset; }
            set { offset = value; NotifyPropertyChanged(); }
        }
        public int Time
        {
            get { return time; }
        }
        public int TimeSelected
        {
            get
            {
                switch (Time)
                {
                    case 1:
                        return 0;
                    case 60:
                        return 1;
                    case 3600:
                        return 2;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (value)
                {
                    case 0:
                        time = 1;
                        break;
                    case 1:
                        time = 60;
                        break;
                    case 2:
                        time = 3600;
                        break;
                    default:
                        time = 1;
                        break;
                }
            }
        }
        public bool IsSelected { get; set; } = true;
        public ObservableCollection<Element> Children { get; set; } = new ObservableCollection<Element>();

        public ObservableCollection<Nodes> TreeNodes {
            get { return Children.Where(x => x is Nodes).Cast<Nodes>().ToObservableCollection(); }
        }
        public MatrixValue[] Objectives {
            get { return objectives; }
            set { objectives = value; }
        }

        public ObservableCollection<Amount> Inputs
        {
            get { return inputs; }
            set { inputs = value; }
        }

        public void Add(Element node, int index = -1)
        {
            if (node == null) return; // TODO exception
            if (node is Node && Children.Count == 0) node.Count = 1;
            if (index != -1)
            {
                Children.Insert(index, node);
            }
            else
            {
                Children.Add(node);
            }
            node.Parent = this;
            UpdateItems();
        }
        public void Remove(Element node)
        {
            Children.Remove(node);
            //foreach (Amount product in node.Products)
            //{
            //    RemoveInput(product);
            //}
            UpdateItems();
        }

        public void MoveUp(Element node, int step)
        {
            this.Children.MoveStep(node, step);
        }
        public void MoveDown(Element node, int step)
        {
            this.Children.MoveStep(node, -step);
        }

        public Nodes UpLevelNode(Element node)
        {
            int index = Children.IndexOf(node);
            Children.RemoveAt(index);
            Nodes newNodes = new Nodes(database);
            newNodes.Add(node);
            newNodes.Parent = this;
            if (Children.Count == 0)
            {
                Children.Add(newNodes);
            }
            else
            {
                Children.Insert(index, newNodes);
            }
            return newNodes;
        }
        public void DownLevelNode(Element node)
        {
            if (Parent != null)
            {
                int index = Children.IndexOf(node);
                Children.RemoveAt(index);
                int afterIndex = Parent.Children.IndexOf(this);
                Parent.Children.Insert(afterIndex + 1, node);
                node.Parent = Parent;
            }
        }
        public void SetInput(Amount amount)
        {
            var input = inputs.Where(x => x.Type == amount.Type && x.Name == amount.Name).FirstOrDefault();
            if(input != null)
            {
                if(amount.Count == 0)
                {
                    RemoveInput(amount);
                }
                else
                {
                    input.Count = amount.Count;
                }
            }
            else
            {
                inputs.Add(amount.Clone());
            }
        }
        public void RemoveInput(Amount amount)
        {
            if (inputs != null)
            {
                inputs = inputs.Where(element => element.Name != amount.Name).ToObservableCollection();
            }
        }
        public double GetInputValue(Amount amount)
        {
            if (inputs != null)
            {
                foreach (Amount input in inputs)
                {
                    if (input.Name == amount.Name) return input.Count;
                }
            }
            return amount.Count;
        }

        /// <summary>
        /// Copy Input after Reset objectives
        /// </summary>
        public void CopyInputsToObjectives()
        {
            objectives = null;
            if (inputs != null && inputs.Count > 0)
            {
                foreach (Amount input in inputs)
                {
                    objectives = AddMatrixValue(objectives, input, true);
                }
            }
        }

        internal MatrixValue[] AddMatrixValue(MatrixValue[] matrixValues, Amount amount, bool append = false)
        {
            MatrixValue matrixValue = new MatrixValue(amount.Type, amount.Name, amount.Count);
            return AddMatrixValue(matrixValues, matrixValue, append);
        }
        internal MatrixValue[] AddMatrixValue(MatrixValue[] matrixValues, MatrixValue newMatrixValue, bool append = false)
        {
            if (matrixValues == null)
            {
                matrixValues = new MatrixValue[1];
                matrixValues[0] = newMatrixValue;
            }
            else
            {
                bool exist = false;
                foreach (MatrixValue matrixValue in matrixValues)
                {
                    if (matrixValue.Name.Equals(newMatrixValue.Name))
                    {
                        if (append) matrixValue.Value += newMatrixValue.Value;
                        else matrixValue.Value = newMatrixValue.Value;
                        exist = true;
                    }
                }
                if (!exist)
                {
                    Array.Resize(ref matrixValues, matrixValues.Length + 1);
                    matrixValues[matrixValues.Length - 1] = newMatrixValue;
                }
            }
            return matrixValues;
        }
        private void UpdateItems()
        {
            if (Children.Count > 0)
            {
                Icon = Children.First().Icon;
                Name = Children.First().Name;
                Type = Children.First().Type;
                ObservableCollection<Amount> products = new ObservableCollection<Amount>();
                ObservableCollection<Amount> ingredients = new ObservableCollection<Amount>();
                foreach (Element node in Children)
                {
                    foreach (Amount amount in node.Products)
                    {
                        products.Add(amount);
                    }
                    foreach (Amount amount in node.Ingredients)
                    {
                        ingredients.Add(amount);
                    }
                }
                Products = products.Distinct().ToObservableCollection();
                Ingredients = ingredients.Distinct().ToObservableCollection();
            }
            else
            {
                Icon = null;
                Products.Clear();
                Ingredients.Clear();
            }

        }

    }
}
