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
using System.Windows.Shapes;
using UniversalHelmod.Classes;
using UniversalHelmod.Extractors.Stationeers.Models;
using UniversalHelmod.Workspaces.Models;

namespace UniversalHelmod.Extractors.Stationeers
{
    /// <summary>
    /// Logique d'interaction pour StationeersExtractorWindow.xaml
    /// </summary>
    public partial class StationeersExtractorWindow : Window
    {
        public StationeersExtractorWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var model = new SettingsModel();
            model.Path = Properties.Settings.Default.StationeersFolder;
            this.DataContext = model;
        }
        private SettingsModel Model => this.DataContext as SettingsModel;

        private void ButtonDirectory_Click(object sender, RoutedEventArgs e)
        {
            // To use System.Windows.Forms add <UseWindowsForms>true</UseWindowsForms> in .csproj file
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                string path = dialog.SelectedPath;
                Model.Path = path;
                Properties.Settings.Default.StationeersFolder = path;
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            PopulateDatabase();
        }

        private async void PopulateDatabase()
        {
            var result = await RSAdaptater.PopulateDatabaseAsync();
            WorkspacesModel.Intance.Current.Database = result;
            WorkspacesModel.Intance.Current.SaveDatabase();
            WorkspacesModel.Intance.Current.Load();
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
