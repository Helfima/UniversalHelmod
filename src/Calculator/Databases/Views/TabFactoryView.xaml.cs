using Calculator.Databases.Models;
using Calculator.Exceptions;
using Calculator.Extensions;
using Calculator.Workspaces.Models;
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

namespace Calculator.Databases.Views
{
    /// <summary>
    /// Logique d'interaction pour TabFactoryView.xaml
    /// </summary>
    public partial class TabFactoryView : UserControl
    {
        public TabFactoryView()
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
                    Model.Factories = Model.Database.Factories.ToObservableCollection();
                }
                else
                {
                    Model.Factories = Model.Database.Factories.Where(x => x.Type == filter).ToObservableCollection();
                }
            }
        }
        private void ListViewElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var element = this.ListViewItems.SelectedItem as Factory;
            this.Model.SelectedFactory = element;
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
            if (!Model.FactoryTypes.Contains(text))
            {
                Model.FactoryTypes.Add(text);
                combobox.SelectedItem = text;
            }
        }
        private void NewElement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var element = Model.SelectedFactory;
                Model.SelectedFactory = element.Clone();
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
                Model.SelectedFactory = new Factory();
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
                var element = Model.SelectedFactory;
                Model.SaveFactory(element);
                Model.SelectedFactory = element;
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
                var element = Model.SelectedFactory;
                Model.DeleteFactory(element);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
