using UniversalHelmod.Classes;
using UniversalHelmod.Workspaces.Converter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UniversalHelmod.Workspaces.Models
{
    public class WorkspacesModel : NotifyProperty
    {
        private static WorkspacesModel instance;
        public static WorkspacesModel Intance => GetInstance();
        private static WorkspacesModel GetInstance()
        {
            if (instance == null)
            {
                instance = new WorkspacesModel();
                instance.Load();
            }
            return instance;
        }
        private ObservableCollection<Workspace> workspaces = new ObservableCollection<Workspace>();
        private Workspace current;
        private Workspace newCurrent = new Workspace();
        private bool isChanging = true;
        private bool isActived = false;

        public ObservableCollection<Workspace> Workspaces
        {
            get { return workspaces; }
            set { workspaces = value; NotifyPropertyChanged(); }
        }
        public Workspace Current
        {
            get { return current; }
            set { current = value; NotifyPropertyChanged(); }
        }
        public Workspace NewCurrent
        {
            get { return newCurrent; }
            set { newCurrent = value; NotifyPropertyChanged(); }
        }
        public bool IsChanging
        {
            get { return isChanging; }
            set { isChanging = value; NotifyPropertyChanged(); }
        }
        public bool IsActived
        {
            get { return isActived; }
            set { isActived = value; NotifyPropertyChanged(); }
        }
        public void CreateWorkspace(string name, string path)
        {
            var workspace = new Workspace() { Name = name, PathFolder = path, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
            workspace.Initialize();
            AddWorkspace(workspace);
        }
        public void DeleteWorkspace(Workspace workspace)
        {
            workspaces.Remove(workspace);
        }
        private void AddWorkspace(Workspace workspace)
        {
            if(this.Workspaces == null) workspaces = new ObservableCollection<Workspace>();
            workspaces.Add(workspace);
        }
        public void Load()
        {
            instance = WorkspaceConverter.ReadXml();
        }
        public void Save()
        {
            WorkspaceConverter.WriteXml(instance);
        }
    }
}
