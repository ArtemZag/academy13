using System;


namespace BinaryStudio.PhotoGallery.Core.Exceptions
{
    class FilePathNotExistException : Exception
    {
        public FilePathNotExistException(string message) : base(message)
        {
        }
    }
}
