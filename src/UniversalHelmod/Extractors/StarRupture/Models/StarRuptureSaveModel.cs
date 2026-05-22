using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UniversalHelmod.Classes;

namespace UniversalHelmod.Extractors.StarRupture.Models
{
    internal class StarRuptureSaveModel : NotifyProperty
    {
        public StarRuptureSaveModel()
        {
            MessageSent += MessageEvent_MessageSent;
        }

        public static event EventHandler<MessageEventArgs> MessageSent;
        public static void InvokeMessage(object sender, string message)
        {
            MessageSent?.Invoke(sender, new MessageEventArgs(message));
        }
        private void MessageEvent_MessageSent(object sender, MessageEventArgs e)
        {
            this.Message = e.Message;
        }
        private string message;
        public string Message
        {
            get { return this.message; }
            set { this.message = value; NotifyPropertyChanged(); }
        }
        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<FileSave> fileSaves = new ObservableCollection<FileSave>();
        public ObservableCollection<FileSave> FileSaves
        {
            get { return this.fileSaves; }
            set { this.fileSaves = value; NotifyPropertyChanged(); }
        }
    }
}
