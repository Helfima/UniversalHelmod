using Calculator.Databases.Models;
using Calculator.Extensions;
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
    /// Logique d'interaction pour TabItemView.xaml
    /// </summary>
    public partial class TabItemView : UserControl
    {
        public TabItemView()
        {
            InitializeComponent();
        }
        private DatabaseModel Model => DataContext as DatabaseModel;

        private void ItemFilter_Checked(object sender, RoutedEventArgs e)
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
                    Model.Items = Model.Database.Items.Where(x => x.ItemType == filter).ToObservableCollection();
                }
            }
        }
        private void ListViewItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = this.ListViewItems.SelectedItem as Item;
            this.Model.SelectedItem = item;
        }
        private void ItemType_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var combobox = sender as ComboBox;
                var text = combobox.Text;
                if (!Model.ItemTypes.Contains(text))
                {
                    Model.ItemTypes.Add(text);
                    combobox.SelectedItem = text;
                }
            }
        }
        private void ItemForm_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var combobox = sender as ComboBox;
                var text = combobox.Text;
                if (!Model.ItemForms.Contains(text))
                {
                    Model.ItemForms.Add(text);
                    combobox.SelectedItem = text;
                }
            }
        }
    }
}
