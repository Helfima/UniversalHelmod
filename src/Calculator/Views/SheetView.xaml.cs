using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Calculator.Math;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Calculator.Converter;
using System.Threading;
using System.Globalization;
using Calculator.Protos.FGProtos;

namespace Calculator.Views
{
    /// <summary>
    /// Logique d'interaction pour Sheet.xaml
    /// </summary>
    public partial class SheetView : UserControl
    {
        
        public SheetView()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            var database = FGDatabase.Intance;

            DataModel model = DataModelConverter.ReadXml();
            if (model != null)
            {
                this.DataContext = model;
            }
            else
            {
                this.DataContext = new DataModel();
            }
            Compute();
            Refresh();
        }

        private RecipeSelector recipeSelector;

        private RecipeChoose recipeChoose;

        private EditionInput editionInput;
        private DataModel Model => (DataModel)this.DataContext;
        private bool AddNearRow = false;

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            var source = (dynamic)e.Source;
            var item = source.DataContext as Amount;
            if (item != null)
            {
                OpenEditionInput(item);
            }
        }
        private void PowerShard_Click(object sender, RoutedEventArgs e)
        {
            var source = (dynamic)e.Source;
            var recipe = source.DataContext as Node;
            var button = (dynamic)sender;
            switch (button.Name)
            {
                case "PowerShard1":
                    if(recipe.Builder.PowerShard == 1) recipe.Builder.PowerShard = 0;
                    else recipe.Builder.PowerShard = 1;
                    break;
                case "PowerShard2":
                    if (recipe.Builder.PowerShard == 2) recipe.Builder.PowerShard = 0;
                    else recipe.Builder.PowerShard = 2;
                    break;
                case "PowerShard3":
                    if (recipe.Builder.PowerShard == 3) recipe.Builder.PowerShard = 0;
                    else recipe.Builder.PowerShard = 3;
                    break;
            }
            Compute();
        }

        private void NewSheet_Click(object sender, RoutedEventArgs e)
        {
            AddSheet();
            OpenRecipeSelector();
        }
        private void DeleteSheet_Click(object sender, RoutedEventArgs e)
        {
            DeleteSheet();
        }
        private void OpenEditionInput(Amount item)
        {
            if (editionInput != null) editionInput.Close();
            editionInput = new EditionInput();
            editionInput.Item = item;
            editionInput.CurrentNode = Model.CurrentNode;
            editionInput.EditionValidated += EditionInput_EditionValidated;
            editionInput.Show();
        }

        private void EditionInput_EditionValidated(object sender, RoutedEventArgs e)
        {
            Compute();
            Refresh();
        }

        private void OpenRecipeSelector_Click(object sender, RoutedEventArgs e)
        {
            this.AddNearRow = Keyboard.Modifiers == ModifierKeys.Control;
            OpenRecipeSelector();
        }
        private void OpenRecipeSelector()
        {
            if (recipeSelector != null) recipeSelector.Close();
            recipeSelector = new RecipeSelector();
            recipeSelector.ViewRecipes.SelectionChanged += RecipeSelector_SelectionChanged;
            recipeSelector.Show();
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Compute();
            Refresh();
        }
        private void Ingredient_Click(object sender, RoutedEventArgs e)
        {
            
            this.AddNearRow = Keyboard.Modifiers == ModifierKeys.Control;
            var source = (dynamic) e.Source;
            var amount = source.DataContext as Amount;
            if(amount != null)
            {
                AddNode(amount.Item);
            }
        }
        private void DeleteNode_Click(object sender, RoutedEventArgs e)
        {
            var source = (dynamic)e.Source;
            var node = source.DataContext as Element;
            if (node != null && Model.CurrentNode != null)
            {
                Model.CurrentNode.Remove(node);
                Compute();
                Refresh();
            }
        }
        private void DownNode_Click(object sender, RoutedEventArgs e)
        {
            var source = (dynamic)e.Source;
            var node = source.DataContext as Element;
            if (node != null && Model.CurrentNode != null)
            {
                Model.CurrentNode.MoveUp(node, 1);
                Compute();
                Refresh();
            }
        }
        private void UpNode_Click(object sender, RoutedEventArgs e)
        {
            var source = (dynamic)e.Source;
            var node = source.DataContext as Element;
            if (node != null && Model.CurrentNode != null)
            {
                Model.CurrentNode.MoveDown(node, 1);
                Compute();
                Refresh();
            }
        }
        private void UpLevelNode_Click(object sender, RoutedEventArgs e)
        {
            var source = (dynamic)e.Source;
            var node = source.DataContext as Element;
            if (node != null && node.Parent != null)
            {
                var selection = node.Parent.UpLevelNode(node);
                Model.CurrentNode = selection;
                Compute();
                Refresh();
            }
        }
        private void DownLevelNode_Click(object sender, RoutedEventArgs e)
        {
            var source = (dynamic)e.Source;
            var node = source.DataContext as Element;
            if (node != null && node.Parent != null)
            {
                node.Parent.DownLevelNode(node);
                Compute();
                Refresh();
            }
        }
        private void AddNode(Item item)
        {
            if (item != null)
            {
                List<Recipe> recipes = Database.Intance.SelectRecipeByProduct(item);
                if (recipes != null && recipes.Count > 0)
                {
                    if (recipes.Count == 1)
                    {
                        Recipe newRecipe = recipes.First();
                        AddNode(newRecipe);
                    }
                    else
                    {
                        if (recipeChoose != null) recipeChoose.Close();
                        recipeChoose = new RecipeChoose();
                        recipeChoose.ViewRecipes.SelectionChanged += RecipeSelector_SelectionChanged;
                        recipeChoose.SetRecipeByProduct(item);
                        recipeChoose.Show();
                    }
                }
            }

        }
        private void AddSheet()
        {
            var newSheet = new Nodes(60);
            Model.Sheets.Add(newSheet);
            Model.CurrentSheet = newSheet;
            Model.CurrentNode = newSheet;
        }
        private void DeleteSheet()
        {
            Model.Sheets.Remove(Model.CurrentSheet);
            if (Model.Sheets.Count > 0)
            {
                var newSheet = Model.Sheets.LastOrDefault();
                Model.CurrentSheet = newSheet;
                Model.CurrentNode = newSheet;
            }
            else
            {
                AddSheet();
            }
        }
        private void AddNode(Recipe recipe)
        {
            var newNode = new Node(recipe);
            AddNode(newNode);
        }
        private void AddNode(Node node)
        {
            if (Model.CurrentSheet == null)
            {
                AddSheet();
            }
            if (AddNearRow)
            {
                int index = this.GridSheet.SelectedIndex + 1;
                Model.CurrentNode.Add(node, index);
            }
            else
            {
                Model.CurrentNode.Add(node);
            }
            Compute();
            Refresh();
            this.GridSheet.SelectedItem = null;
        }
        private void RecipeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selection = e.AddedItems[0] as Recipe;
                if (recipeSelector != null)
                {
                    recipeSelector.Close();
                }
                if (recipeChoose != null)
                {
                    recipeChoose.Close();
                }
                AddNode(selection);
            }
        }

        private void Compute()
        {
            Compute compute = new Compute();
            compute.Update(Model.CurrentSheet);
        }
        private void Refresh()
        {
            Model.UpdateFlatNodes();
            this.SheetNavigate.Items.Refresh();
            this.NodeNavigate.Items.Refresh();
            this.GridSheet.Items.Refresh();
            //this.GridInput.Items.Refresh();
            //this.GridOutput.Items.Refresh();
        }

        private void SheetNavigate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selection = e.AddedItems[0] as Nodes;
                Model.CurrentSheet = selection;
                Model.CurrentNode = selection;
                Compute();
                Refresh();
            }
        }
        private void NodeNavigate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selection = e.AddedItems[0] as Nodes;
                Model.CurrentNode = selection;
            }
        }

        private void NodeNavigate_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                var selection = e.NewValue as Nodes;
                Model.CurrentNode = selection;
            }
        }

        public void Save_Click(object sender, RoutedEventArgs e)
        {
            DataModelConverter.WriteXml(Model);
        }
    }
}
