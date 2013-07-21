using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class AlbumNotFoundException : Exception
    {
        public AlbumNotFoundException(Exception innerException) : base(string.Empty, innerException)
        {
        }
    }
}
