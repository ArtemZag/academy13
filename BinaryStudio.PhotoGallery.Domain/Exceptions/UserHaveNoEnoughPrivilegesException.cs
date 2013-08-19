using System;


namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    class UserHaveNoEnoughPrivilegesException : Exception
    {
        public UserHaveNoEnoughPrivilegesException(string message) : base(message)
        {
        }
    }
}
