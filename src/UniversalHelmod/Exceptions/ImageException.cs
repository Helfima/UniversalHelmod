using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Exceptions
{
    public class ImageException : Exception
    {
        public ImageException() : base() { }
        public ImageException(string message) : base(message) { }
        public ImageException(string message, Exception innerException) : base(message, innerException) { }
    }
}
