using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Classes
{
    public class MessageEventArgs : EventArgs
    {
        private string message;
        public MessageEventArgs(string message)
        {
            this.message = message;
        }
        public string Message => this.message;
    }
}
