using BinaryStudio.PhotoGallery.Database;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    abstract internal class Service
    {
        protected readonly IUnitOfWorkFactory workFactory;

        protected Service(IUnitOfWorkFactory workFactory)
        {
            this.workFactory = workFactory;
        }
    }
}
