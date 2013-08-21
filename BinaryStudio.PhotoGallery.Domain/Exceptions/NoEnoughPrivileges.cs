using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class NoEnoughPrivilegesException : Exception
    {
        public NoEnoughPrivilegesException(string message) : base(message)
        {
        }
    }
}


