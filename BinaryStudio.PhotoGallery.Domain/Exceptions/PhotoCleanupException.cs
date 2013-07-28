using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class PhotoCleanupException : Exception
    {
        public PhotoCleanupException(Exception innerException) : base(string.Empty, innerException)
        {
        }
    }
}
