using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class GroupRepository : BaseRepository<GroupModel>, IGroupRepository
    {
        public GroupRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }
    }
}
