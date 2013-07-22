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

        public void AddAvailableGroup(int groupID, int albumID)
        {
            var group = new AvailableGroupModel()
                {
                    GroupModelID = groupID,
                    AlbumModelID = albumID
                };
            base.Create(group);
        }
    }
}
