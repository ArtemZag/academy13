using System;

namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public string UserEmail { get; private set; }

        public UserNotFoundException(string userEmail, Exception innerException)
            : base(string.Empty, innerException)
        {
            UserEmail = userEmail;
        }
    }
}