using UniversalHelmod.Workspaces.Models;
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

namespace UniversalHelmod.Workspaces.Views
{
    /// <summary>
    /// Logique d'interaction pour WorkspacesView.xaml
    /// </summary>
    public partial class WorkspacesView : UserControl
    {
        public WorkspacesView()
        {
            InitializeComponent();
            this.DataContext = WorkspacesModel.Intance;
        }
        private WorkspacesModel Model => this.DataContext as WorkspacesModel;
        private void ButtonWorkspaceCreate_Click(object sender, RoutedEventArgs e)
        {
            string name = this.WorkspaceName.Text;
            string path = this.WorkspacePath.Text;
            Model.CreateWorkspace(name, path);
            Model.Save();
        }
        private void ButtonWorkspaceDelete_Click(object sender, RoutedEventArgs e)
        {
            var source = (dynamic)e.Source;
            var workspace = source.DataContext as Workspace;
            Model.DeleteWorkspace(workspace);
            Model.Save();
        }
        private void ButtonDirectory_Click(object sender, RoutedEventArgs e)
        {
            // To use System.Windows.Forms add <UseWindowsForms>true</UseWindowsForms> in .csproj file
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                string path = dialog.SelectedPath;
                this.WorkspacePath.Text = path;
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var workspace = this.ListWorkspaces.SelectedItem as Workspace;
            Model.Current = workspace;
            workspace.Load();
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
