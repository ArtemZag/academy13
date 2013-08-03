using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; private set; }
    }
}