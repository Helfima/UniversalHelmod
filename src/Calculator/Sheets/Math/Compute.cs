using Calculator.Enums;
using Calculator.Extensions;
using Calculator.Databases.Models;
using Calculator.Sheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Sheets.Math
{
    public class Compute
    {
        public static double EPSILON = 0.01;
        private Solver solver;
        private int Time = 1;

        public Compute()
        {
            //this.solver = new SolverAlgebra();
            this.solver = new SolverSimplex();
        }
        public void Update(Nodes nodes)
        {
            if (nodes == null) return;
            Time = nodes.Time;
            nodes.Count = 1;
            ComputeNode(nodes);
        }
        
        private void ComputeNode(Nodes nodes)
        {

            if (nodes == null || nodes.Children == null || nodes.Children.Count == 0) return;
            nodes.Objectives = null;
            nodes.CopyInputsToObjectives();
            foreach (Element child in nodes.Children)
            {
                if (child is Node)
                {
                    Node node = child as Node;
                    node.Initialize();
                    node.UpdateEffect();
                }
                if (child is Nodes)
                {
                    Nodes childNodes = child as Nodes;
                    ComputeNode(childNodes);
                }
            }
            Matrix matrix = GetMatrix(nodes);
            // prepare des objectifs manquants
            if (nodes.Objectives == null)
            {
                var items = nodes.Children.First().Products;
                nodes.Objectives = new MatrixValue[items.Count];
                for (int i = 0; i < items.Count; i++)
                {
                    Amount item = items[i];
                    nodes.Objectives[i] = new MatrixValue(item.Type, item.Name, item.Count);
                }
            }

            MatrixValue[] result = solver.Solve(matrix, nodes.Objectives);
            foreach (Element child in nodes.Children)
            {
                foreach (MatrixValue value in result)
                {
                    if (!value.IsUsed && child.Match(value))
                    {
                        child.Count = value.Value;
                        value.IsUsed = true;
                        break;
                    }
                }
                ComputeItem(child);
            }
            ComputeFactory(nodes);
            ComputePower(nodes);
            ComputeInputOutput(nodes);
        }
        private void ComputeItem(Element element)
        {
            foreach (Amount amount in element.Ingredients)
            {
                amount.Count = amount.Count * element.Count;
            }
            foreach (Amount amount in element.Products)
            {
                amount.Count = amount.Count * element.Count;
            }
        }
        private void ComputePower(Nodes nodes)
        {
            nodes.Power = 0;
            foreach (Element child in nodes.Children)
            {
                nodes.Power += child.Power;
            }
            nodes.Power = nodes.Power * nodes.Count;
        }

        private void ComputeFactory(Nodes nodes)
        {
            foreach (Element child in nodes.Children)
            {
                if (child is Node)
                {
                    Node node = (Node)child;
                    if (node.Builder != null)
                    {
                        double speed = node.Builder.Speed;
                        node.Builder.Count = node.Recipe.Energy * node.Count / (speed * node.Effects.Speed * Time);
                        node.Power = node.Builder.Count * node.Builder.Power * node.Effects.Consumption;
                    }
                }
            }
        }

        private void ComputeInputOutput(Nodes nodes)
        {
            Dictionary<string, Amount> products = new Dictionary<string, Amount>();
            Dictionary<string, Amount> ingredients = new Dictionary<string, Amount>();
            foreach (Element child in nodes.Children)
            {
                foreach (Amount amount in child.Products)
                {
                    if (products.ContainsKey(amount.Name))
                    {
                        products[amount.Name].Count += amount.Count;
                    }
                    else
                    {
                        products.Add(amount.Name, amount.Clone());
                    }
                }
                foreach (Amount amount in child.Ingredients)
                {
                    if (ingredients.ContainsKey(amount.Name))
                    {
                        ingredients[amount.Name].Count += amount.Count;
                    }
                    else
                    {
                        ingredients.Add(amount.Name, amount.Clone());
                    }
                }
            }

            //consomme les produits
            foreach (KeyValuePair<string, Amount> entry in ingredients)
            {
                if (products.ContainsKey(entry.Key))
                {
                    double productCount = products[entry.Key].Count;
                    double ingredientCount = entry.Value.Count;
                    products[entry.Key].Count = System.Math.Max(0, products[entry.Key].Count - ingredientCount);
                    ingredients[entry.Key].Count = System.Math.Max(0, ingredients[entry.Key].Count - productCount);
                }
            }
            nodes.Products = products.Select(entry => entry.Value).Where(x => x.Count > 0.0001).ToObservableCollection();
            nodes.Ingredients = ingredients.Select(entry => entry.Value).Where(x => x.Count > 0.0001).ToObservableCollection();
        }

        private Matrix GetMatrix(Nodes nodes)
        {
            List<MatrixHeader> rowHeaders = new List<MatrixHeader>();
            List<MatrixRow> rowDatas = new List<MatrixRow>();

            List<string> products = new List<string>();
            List<string> ingredients = new List<string>();
            foreach (Element child in nodes.Children)
            {
                double productivity = 1;
                bool isExtractor = false;
                if (child is Node)
                {
                    var node = child as Node;
                    productivity = node.Effects.Productivity;
                    isExtractor = node.Builder.Factory is Extractor;
                }

                rowHeaders.Add(new MatrixHeader(child.Type, child.Name, child.Production));
                MatrixRow rowData = new MatrixRow(child.Type, child.Name);
                foreach (Amount item in child.Products)
                {
                    products.Add(item.Name);
                    rowData.AddValue(new MatrixValue(item.Type, item.Name, item.Count * productivity));
                }
                foreach (Amount item in child.Ingredients)
                {
                    if (!isExtractor)
                    {
                        ingredients.Add(item.Name);
                        rowData.AddValue(new MatrixValue(item.Type, item.Name, -item.Count));
                    }
                }
                rowDatas.Add(rowData);
            }
            // update status
            foreach (Element node in nodes.Children)
            {
                foreach (Amount item in node.Products)
                {
                    if (ingredients.Contains(item.Name))
                    {
                        item.State = ItemState.Residual;
                    }
                    else
                    {
                        item.State = ItemState.Main;
                    }
                }
                foreach (Amount item in node.Ingredients)
                {
                    if (products.Contains(item.Name))
                    {
                        item.State = ItemState.Residual;
                    }
                    else
                    {
                        item.State = ItemState.Main;
                    }
                }
            }
            Matrix matrix = new Matrix(rowHeaders.ToArray(), rowDatas.ToArray());
            return matrix;
        }

        public void ComputeOptimal(Nodes nodes)
        {
            Time = nodes.Time;
            nodes.Count = 1;
            if (nodes == null || nodes.Children == null || nodes.Children.Count == 0) return;
            nodes.Objectives = null;
            nodes.CopyInputsToObjectives();
            Matrix matrix = GetMatrix(nodes.Database);
            MatrixValue[] result = solver.Solve(matrix, nodes.Objectives);
            foreach (Element child in nodes.Children)
            {
                foreach (MatrixValue value in result)
                {
                    if (!value.IsUsed && child.Match(value))
                    {
                        child.Count = value.Value;
                        value.IsUsed = true;
                        break;
                    }
                }
                ComputeItem(child);
            }
            ComputeFactory(nodes);
            ComputePower(nodes);
            ComputeInputOutput(nodes);
        }
        private Matrix GetMatrix(Database database)
        {
            List<MatrixHeader> rowHeaders = new List<MatrixHeader>();
            List<MatrixRow> rowDatas = new List<MatrixRow>();

            List<string> products = new List<string>();
            List<string> ingredients = new List<string>();
            foreach (Recipe recipe in database.Recipes)
            {
                var factory = database.Factories.FirstOrDefault(x => recipe.MadeIn.Contains(x.Name));
                if (factory == null) continue;
                double productivity = 1;
                bool isExtractor = factory is Extractor;

                rowHeaders.Add(new MatrixHeader(recipe.Type, recipe.Name, recipe.Production));
                MatrixRow rowData = new MatrixRow(recipe.Type, recipe.Name);
                foreach (Amount item in recipe.Products)
                {
                    products.Add(item.Name);
                    rowData.AddValue(new MatrixValue(item.Type, item.Name, item.Count * productivity));
                }
                foreach (Amount item in recipe.Ingredients)
                {
                    if (!isExtractor)
                    {
                        ingredients.Add(item.Name);
                        rowData.AddValue(new MatrixValue(item.Type, item.Name, -item.Count));
                    }
                }
                rowDatas.Add(rowData);
            }
            // update status
            foreach (Recipe recipe in database.Recipes)
            {
                foreach (Amount item in recipe.Products)
                {
                    if (ingredients.Contains(item.Name))
                    {
                        item.State = ItemState.Residual;
                    }
                    else
                    {
                        item.State = ItemState.Main;
                    }
                }
                foreach (Amount item in recipe.Ingredients)
                {
                    if (products.Contains(item.Name))
                    {
                        item.State = ItemState.Residual;
                    }
                    else
                    {
                        item.State = ItemState.Main;
                    }
                }
            }
            Matrix matrix = new Matrix(rowHeaders.ToArray(), rowDatas.ToArray());
            return matrix;
        }
    }
}
