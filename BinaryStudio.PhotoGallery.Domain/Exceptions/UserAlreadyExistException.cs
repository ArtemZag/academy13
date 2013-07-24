using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class UserAlreadyExistException : Exception
    {
        public string UserEmail { get; private set; }

        public UserAlreadyExistException(string userEmail)
        {
            UserEmail = userEmail;
        }
    }
}
