using System;


namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class NoEnoughPrivileges : Exception
    {
        public NoEnoughPrivileges(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}


