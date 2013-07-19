using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Database
{
    internal interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
    }
}
