using UniversalHelmod.Classes;
using UniversalHelmod.Views;
using UniversalHelmod.Workspaces.Models;
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

namespace UniversalHelmod
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
            if(WorkspacesModel.Intance.Current == null)
            {
                MenuWorkspace_Click(sender, e);
            }
        }
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MenuWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var view = new Workspaces.WorkspacesWindow();
            //view.Owner = this;
            view.Show();
        }
        public void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            WorkspacesModel.Intance.Current.SaveDataModel();
        }
        private void MenuItemDatabaseSave_Click(object sender, RoutedEventArgs e)
        {
            WorkspacesModel.Intance.Current.SaveDatabase();
        }
        private void MenuItemDatabase_Click(object sender, RoutedEventArgs e)
        {
            var view = new Databases.DatabaseWindow();
            view.Show();
        }
        private void MenuItemExtract_Click(object sender, RoutedEventArgs e)
        {
            var view = new Extractors.Satisfactory.SatisfactoryExtractorWindow();
            view.Show();
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
