using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniversalHelmod.Classes;
using UniversalHelmod.Databases.Models;
using UniversalHelmod.Exceptions;
using UniversalHelmod.Extensions;
using UniversalHelmod.Extractors.Stationeers.Models;
using UniversalHelmod.Workspaces.Models;

namespace UniversalHelmod.Databases.Views
{
    /// <summary>
    /// Logique d'interaction pour TabRecipeView.xaml
    /// </summary>
    public partial class TabRecipeView : UserControl
    {
        public TabRecipeView()
        {
            InitializeComponent();
        }
        private DatabaseModel Model => DataContext as DatabaseModel;

        private void ElementFilter_Checked(object sender, RoutedEventArgs e)
        {
            var button = e.Source as RadioButton;
            if (button != null)
            {
                Model.RecipeFilter = button.Content.ToString();
            }
        }
        private void ListViewElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var element = this.ListViewItems.SelectedItem as Recipe;
            this.Model.SelectedRecipe = element;
        }
        private void ElementType_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ElementType_LostFocus(sender, null);
            }
        }
        private void ElementType_LostFocus(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            var text = combobox.Text;
            if (!Model.ItemTypes.Contains(text))
            {
                Model.ItemTypes.Add(text);
                combobox.SelectedItem = text;
            }
        }
        private void NewElement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var element = Model.SelectedRecipe;
                Model.SelectedRecipe = element.Clone();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EraserElement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.SelectedRecipe = new Recipe();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveElement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var element = Model.SelectedRecipe;
                Model.SaveRecipe(element);
                Model.SelectedRecipe = element;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteElement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var element = Model.SelectedRecipe;
                Model.DeleteRecipe(element);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelecElementIconPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var element = Model.SelectedRecipe;
                Utils.SelecElementIconPath(element);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = ProductSelector.SelectedItem as Item;
                var amountString = ProductAmount.Text;
                var amount = System.Convert.ToDouble(amountString);
                var itemAmount = new Amount(item, amount);
                Model.SelectedRecipe.Products.Add(itemAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = IngredientSelector.SelectedItem as Item;
                var amountString = IngredientAmount.Text;
                var amount = System.Convert.ToDouble(amountString);
                var itemAmount = new Amount(item, amount);
                Model.SelectedRecipe.Ingredients.Add(itemAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ElementFactory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = ElementFactory.SelectedItem as Factory;
                if (item == null) return;
                Model.SelectedRecipe.MadeIn.Add(item.Name);
                ElementFactory.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveMadeIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag != null)
                {
                    var factoryToRemove = (string)button.Tag;
                    Model.SelectedRecipe.MadeIn.Remove(factoryToRemove);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag != null)
                {
                    var itemToRemove = button.Tag as Amount;
                    Model.SelectedRecipe.Products.Remove(itemToRemove);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag != null)
                {
                    var itemToRemove = button.Tag as Amount;
                    Model.SelectedRecipe.Ingredients.Remove(itemToRemove);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ItemSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = ItemSelector.SelectedItem as Item;
                if (item == null) return;
                Model.SelectedRecipe.Name = item.Name;
                Model.SelectedRecipe.DisplayName = item.DisplayName;
                Model.SelectedRecipe.IconPath = item.IconPath;
                if (Model.SelectedRecipe.Products.Count == 0)
                {
                    var itemAmount = new Amount(item, 1);
                    Model.SelectedRecipe.Products.Add(itemAmount);
                }
                ItemSelector.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ButtonClearFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.RecipeFilter = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
