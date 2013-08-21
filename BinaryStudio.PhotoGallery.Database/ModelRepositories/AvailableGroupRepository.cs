using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    internal class AvailableGroupRepository : BaseRepository<AvailableGroupModel>, IAvailableGroupRepository
    {
        public AvailableGroupRepository(DatabaseContext dataBaseContext)
            : base(dataBaseContext)
        {
        }

        public void Add(int groupId, int albumId)
        {
            base.Add(new AvailableGroupModel(groupId, albumId));
        }
    }
}
