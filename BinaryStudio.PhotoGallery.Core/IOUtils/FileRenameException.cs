using System;

namespace BinaryStudio.PhotoGallery.Core.IOUtils
{
    public class FileRenameException : Exception
    {
        public FileRenameException(string message)
            : base(message, null)
        {
        }
    }
}