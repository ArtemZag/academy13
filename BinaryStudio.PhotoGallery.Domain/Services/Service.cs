using BinaryStudio.PhotoGallery.Database;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    abstract internal class Service
    {
        protected readonly IUnitOfWorkFactory WorkFactory;

        protected Service(IUnitOfWorkFactory workFactory)
        {
            this.WorkFactory = workFactory;
        }
    }
}
