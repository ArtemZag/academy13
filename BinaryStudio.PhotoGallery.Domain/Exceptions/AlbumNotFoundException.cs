using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class AlbumNotFoundException : Exception
    {
        public AlbumNotFoundException(string message) : base(message)
        {
        }
    }
}