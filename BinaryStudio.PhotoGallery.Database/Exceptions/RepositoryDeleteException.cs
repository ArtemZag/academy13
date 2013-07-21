using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Database.Exceptions
{
    class RepositoryDeleteException : Exception
    {
        public RepositoryDeleteException(string entry, Exception innerException)
        {
            var message = "Repository cann't delete " + entry + ". Maybe it is already deleted.";
        }
    }
}
