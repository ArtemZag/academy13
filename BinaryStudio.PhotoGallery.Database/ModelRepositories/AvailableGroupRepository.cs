using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class AvailableGroupRepository : BaseRepository<AvailableGroupModel>, IAvailableGroupRepository
    {
        public AvailableGroupRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }
    }
}
