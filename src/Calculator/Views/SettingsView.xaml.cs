using Calculator.Classes;
using System;
using System.Collections.Generic;
using System.Reflection;
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
    /// Logique d'interaction pour ExtractUModel.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        private MainWindow main;
        public SettingsView(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
        }

        private SettingsModel Model => this.DataContext as SettingsModel;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var model = new SettingsModel();
            model.Path = Properties.Settings.Default.GameFolder;
            this.DataContext = model;
        }

        class SettingsModel : NotifyProperty
        {
            private string path;
            public string Path {
                get { return path; }
                set { path = value; NotifyPropertyChanged(); }
            }
        }

        private void ButtonDirectory_Click(object sender, RoutedEventArgs e)
        {
            // To use System.Windows.Forms add <UseWindowsForms>true</UseWindowsForms> in .csproj file
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                string path = dialog.SelectedPath;
                Model.Path = path;
                Properties.Settings.Default.GameFolder = path;
                
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            Utils.ExtractImages();
            IsValidated = true;
            this.Close();
            main.LoadData();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            IsValidated = false;
            this.Close();
        }

        public bool IsValidated { get; set; }
    }
}
