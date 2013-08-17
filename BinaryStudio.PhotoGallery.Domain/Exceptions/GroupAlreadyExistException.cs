using System;


namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    class GroupAlreadyExistException : Exception
    {
        public GroupAlreadyExistException(string message) : base(message)
        {
        }
    }
}
