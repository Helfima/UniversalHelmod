using Calculator.Classes;
using Calculator.Converter;
using Calculator.Models;
using Calculator.Protos.FGProtos;
using Calculator.Views;
using Calculator.Workspaces;
using Calculator.Workspaces.Models;
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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MenuWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var view = new WorkspacesWindow();
            //view.Owner = this;
            view.Show();
        }
        private void MenuDatabaseLoad_Click(object sender, RoutedEventArgs e)
        {
            this.LoadData();
        }
        public void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            this.SheetView.MenuItemSave_Click(sender, e);
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

        private void MenuItemDatabase_Click(object sender, RoutedEventArgs e)
        {
            var view = new DatabaseView();
            view.Show();
        }
        private void MenuItemDatabaseSave_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConverter.WriteJson(Database.Intance);
        }
        private void MenuItemExtract_Click(object sender, RoutedEventArgs e)
        {
            LaunchSettingsView();
        }
        private void LaunchSettingsView()
        {

            var settingsView = new SettingsView(this);
            settingsView.ShowDialog();
        }
        class MainModel : NotifyProperty
        {
            private NotifyProperty currentModel;
            public NotifyProperty CurrentModel
            {
                get { return currentModel; }
                set { currentModel = value; NotifyPropertyChanged(); }
            }
            public MainModel()
            {
                currentModel = WorkspacesModel.Intance;
            }
        }
    }
}
