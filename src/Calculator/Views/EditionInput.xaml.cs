using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calculator.Views
{
    /// <summary>
    /// Logique d'interaction pour EditionInput.xaml
    /// </summary>
    public partial class EditionInput : Window
    {
        public Amount Item;
        public Nodes CurrentNode;
        public EditionInput()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var value = CurrentNode.GetInputValue(Item);
            this.Quantity.Text = value.ToString(CultureInfo.InvariantCulture);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double result;
            double.TryParse(this.Quantity.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            Item.Count = result;
            CurrentNode.SetInput(Item);
            EditionValidated(sender, e);
            this.Close();
        }

        public event EventHandler<RoutedEventArgs> EditionValidated;
    }
}
