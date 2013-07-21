using System;

namespace BinaryStudio.PhotoGallery.Database.Exceptions
{
    public class RepositoryDeleteException : Exception
    {
        public RepositoryDeleteException(Exception innerException) : base(string.Empty, innerException)
        {
        }
    }
}
