using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class CleanupException : Exception
    {
        public CleanupException(Exception innerException) : base(string.Empty, innerException)
        {
        }
    }
}
