using System;


namespace BinaryStudio.PhotoGallery.Domain.Exceptions
{
    public class GroupNotFoundException : Exception
    {
        public GroupNotFoundException(string message)
            : base(message)
        {
        }
    }
}
