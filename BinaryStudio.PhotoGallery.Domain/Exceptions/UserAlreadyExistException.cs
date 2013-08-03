using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; private set; }
    }
}