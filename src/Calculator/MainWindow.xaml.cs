using Calculator.Classes;
using Calculator.Protos.FGProtos;
using Calculator.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            SheetView.Save_Click(sender, e);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItemDatabase_Click(object sender, RoutedEventArgs e)
        {
            var view = new DatabaseView();
            view.Show();
        }
        private void MenuItemExtract_Click(object sender, RoutedEventArgs e)
        {
            LaunchSettingsView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            if (!File.Exists(FGDatabase.Filename) || !Directory.Exists(Utils.ImagesFolder()))
            {
                LaunchSettingsView();
            }
            else
            {
                this.SheetView.LoadData();
            }
        }
        private void LaunchSettingsView()
        {
            
            var settingsView = new SettingsView(this);
            settingsView.ShowDialog();
        }
    }
}
