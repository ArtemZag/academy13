using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Database
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private DatabaseContext _databaseContext = new DatabaseContext();

        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork(_databaseContext);
        }
    }
}
