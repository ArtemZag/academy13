using System;

namespace BinaryStudio.PhotoGallery.Database.Exceptions
{
    public class RepositoryCreateException : Exception
    {
        public RepositoryCreateException(Exception innerException) : base(string.Empty, innerException)
        {
        }
    }
}
