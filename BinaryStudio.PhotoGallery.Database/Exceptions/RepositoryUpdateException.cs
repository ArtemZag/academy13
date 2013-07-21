using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Database.Exceptions
{
    class RepositoryUpdateException : Exception
    {
        public RepositoryUpdateException(string entry, Exception innerException)
        {
            var message = "Repository cann't update " + entry + ". Maybe it is not present in database...";
        }
    }
}
