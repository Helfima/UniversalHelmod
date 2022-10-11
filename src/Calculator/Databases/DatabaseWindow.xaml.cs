using Calculator.Classes;
using Calculator.Databases.Models;
using Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calculator.Databases
{
    /// <summary>
    /// Logique d'interaction pour DatabaseWindow.xaml
    /// </summary>
    public partial class DatabaseWindow : Window
    {
        public DatabaseWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DatabaseModel model = new DatabaseModel();
            model.Prepare();
            this.DataContext = model;
        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Model.Save();
        }
        private DatabaseModel Model => DataContext as DatabaseModel;

        private void FactoryFilter_Checked(object sender, RoutedEventArgs e)
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
        private void RecipeFilter_Checked(object sender, RoutedEventArgs e)
        {
            var button = e.Source as RadioButton;
            if (button != null)
            {
                var filter = button.Content.ToString();
                if (filter == "None")
                {
                    Model.Recipes = Model.Database.Recipes.ToObservableCollection();
                }
                else
                {
                    Model.Recipes = Model.Database.Recipes.Where(x => x.ItemType == filter).ToObservableCollection();
                }
            }
        }

        
        private void ItemType_TextChanged(object sender, TextChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            //MessageBox.Show(combobox.Text);
        }
        
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TextBox tBox = (TextBox)sender;
                DependencyProperty prop = TextBox.TextProperty;

                BindingExpression binding = BindingOperations.GetBindingExpression(tBox, prop);
                if (binding != null) { binding.UpdateSource(); }
            }
        }

        
    }
}
