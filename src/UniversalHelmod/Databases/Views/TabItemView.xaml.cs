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
using UniversalHelmod.Workspaces.Models;

namespace UniversalHelmod.Databases.Views
{
    /// <summary>
    /// Logique d'interaction pour TabItemView.xaml
    /// </summary>
    public partial class TabItemView : UserControl
    {
        public TabItemView()
        {
            InitializeComponent();
        }
        private DatabaseModel Model => DataContext as DatabaseModel;

        private void ElementFilter_Checked(object sender, RoutedEventArgs e)
        {
            var button = e.Source as RadioButton;
            if (button != null)
            {
                var filter = button.Content.ToString();
                if (filter == "None")
                {
                    Model.Items = Model.Database.Items.ToObservableCollection();
                }
                else
                {
                    Model.Items = Model.Database.Items.Where(x => x.Type == filter).ToObservableCollection();
                }
            }
        }
        private void ListViewElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = this.ListViewItems.SelectedItem as Item;
            this.Model.SelectedItem = item;
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
            if (String.IsNullOrEmpty(text) == false && Model.ItemTypes.Contains(text) == false)
            {
                Model.ItemTypes.Add(text);
                combobox.SelectedItem = text;
            }
        }
        private void ElementForm_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ElementForm_LostFocus(sender, null);
            }
        }
        private void ElementForm_LostFocus(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            var text = combobox.Text;
            if (String.IsNullOrEmpty(text) == false && Model.ItemForms.Contains(text) == false)
            {
                Model.ItemForms.Add(text);
                combobox.SelectedItem = text;
            }
        }
        private void NewElement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = Model.SelectedItem;
                Model.SelectedItem = item.Clone();
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
                Model.SelectedItem = new Item();
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
                var item = Model.SelectedItem;
                Model.SaveItem(item);
                Model.SelectedItem = item;
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
                var item = Model.SelectedItem;
                Model.DeleteItem(item);
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
                var element = Model.SelectedItem;
                Utils.SelecElementIconPath(element);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
