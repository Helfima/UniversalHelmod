using System;
using System.Collections.Generic;
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

namespace UniversalHelmod.Databases.Views
{
    /// <summary>
    /// Logique d'interaction pour TabLogisticView.xaml
    /// </summary>
    public partial class TabLogisticView : UserControl
    {
        public TabLogisticView()
        {
            InitializeComponent();
        }
        private DatabaseModel Model => DataContext as DatabaseModel;

        private void ElementFilter_Checked(object sender, RoutedEventArgs e)
        {
            var button = e.Source as RadioButton;
            if (button != null)
            {
                Model.LogisticFilter = button.Content?.ToString();
            }
        }
        private void ListViewElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = this.ListViewItems.SelectedItem as Logistic;
            this.Model.SelectedLogistic = item;
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
                var item = Model.SelectedLogistic;
                Model.SelectedLogistic = item.Clone();
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
                Model.SelectedLogistic = new Logistic();
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
                var item = Model.SelectedLogistic;
                Model.SaveLogistic(item);
                Model.SelectedLogistic = item;
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
                var item = Model.SelectedLogistic;
                Model.DeleteLogistic(item);
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
                var element = Model.SelectedLogistic;
                Utils.SelecElementIconPath(element);
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
                Model.LogisticFilter = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
