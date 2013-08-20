using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IAvailableGroupRepository : IBaseRepository<AvailableGroupModel>
    {
        /// <summary>
        /// Adds group that will have some permissions in work with album
        /// </summary>
        /// <param name="groupId">ID of group with permissions</param>
        /// <param name="albumId">Album ID</param>
        void Add(int groupId, int albumId);
    }
}
