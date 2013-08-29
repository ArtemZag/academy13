using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IGroupService
    {
        IEnumerable<AvailableGroupModel> GetAvialableGroups(int userId, int albumId);

        /// <summary>
        /// Creates new group by model
        /// </summary>
        /// <param name="userId">Group owner</param>
        /// <param name="groupModel"></param>
        void Create(int userId, GroupModel groupModel);

        /// <summary>
        /// Creates new group by name
        /// </summary>
        /// <param name="userId">Group owner</param>
        /// <param name="groupName"></param>
        void Create(int userId, string groupName);

        /// <summary>
        /// Gets groupModel by it's ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        GroupModel GetGroup(int groupId);

        /// <summary>
        /// Adds user to group by groupID
        /// </summary>
        /// <param name="ownerId">Group ownerID</param>
        /// <param name="userId">User to add</param>
        /// <param name="groupId"></param>
        void AddUser(int ownerId, int userId, int groupId);

        /// <summary>
        /// Adds user to group by groupName
        /// </summary>
        /// <param name="ownerId">Group ownerID</param>
        /// <param name="userId">User to add</param>
        /// <param name="groupName"></param>
        void AddUser(int ownerId, int userId, string groupName);

        /// <summary>
        /// Remove user from group by its ID
        /// </summary>
        /// <param name="ownerId">Group ownerID</param>
        /// <param name="userId">User to add</param>
        /// <param name="groupId"></param>
        void RemoveUser(int ownerId, int userId, int groupId);

        /// <summary>
        /// Remove user from group by its name
        /// </summary>
        /// <param name="ownerId">Group ownerID</param>
        /// <param name="userId">User to add</param>
        /// <param name="groupName"></param>
        void RemoveUser(int ownerId, int userId, string groupName);

        /// <summary>
        /// Delete group by its ID
        /// </summary>
        /// <param name="ownerId">Group ownerID</param>
        /// <param name="groupId"></param>
        void Delete(int ownerId, int groupId);

    }
}
