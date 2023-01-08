using System;
using System.Collections.Generic;
using System.Text;
using UniversalHelmod.Classes;

namespace UniversalHelmod.Extractors.Stationeers.Models
{
    public class SettingsModel : NotifyProperty
    {
        public SettingsModel()
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

        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; NotifyPropertyChanged(); }
        }
        private string message;
        public string Message
        {
            get { return this.message; }
            set { this.message = value; NotifyPropertyChanged(); }
        }
    }
}
