using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Database
{
    internal class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private IUnitOfWork _unitOfWork;
        private static UnitOfWorkFactory _instance;

        // Hidden constructor realizes singleton
        private UnitOfWorkFactory()
        {
        }

        // lazy load for instance 
        public static IUnitOfWorkFactory Instance
        {
            get { return _instance ?? (_instance = new UnitOfWorkFactory()); }
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return _unitOfWork = new UnitOfWork();
        }
    }
}
