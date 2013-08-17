using System;


namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class AlbumAlreadyExistException : Exception
    {
        public AlbumAlreadyExistException(string message) : base(message)
        {
        }
    }
}
