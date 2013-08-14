using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}