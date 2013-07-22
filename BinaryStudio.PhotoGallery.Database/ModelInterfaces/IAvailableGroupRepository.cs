using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IAvailableGroupRepository : IBaseRepository<AvailableGroupModel>
    {
        /// <summary>
        /// Adds group that will have some permissions in work with album
        /// </summary>
        /// <param name="groupID">ID of group with permissions</param>
        /// <param name="albumID">Album ID</param>
        void AddAvailableGroup(int groupID, int albumID);
    }
}
