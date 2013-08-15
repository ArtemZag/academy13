using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class SynchronizeException : Exception
    {
        public SynchronizeException(Exception innerException): base(string.Empty, innerException)
        {
        }
    }
}
