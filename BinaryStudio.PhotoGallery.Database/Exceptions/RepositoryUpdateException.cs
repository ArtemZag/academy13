using System;

namespace BinaryStudio.PhotoGallery.Database.Exceptions
{
    public class RepositoryUpdateException : Exception
    {
        public RepositoryUpdateException(Exception innerException) : base(string.Empty, innerException)
        {
        }
    }
}
