using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Database.Exceptions
{
    class RepositoryCreateException :Exception
    {
        public RepositoryCreateException(string entry, Exception innerException)
        {
            var message = "Repository cann't create " + entry.ToString();
        }
    }
}
