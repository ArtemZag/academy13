using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    interface IGroupService
    {
        /// <summary>
        /// Creates new group by model
        /// </summary>
        /// <param name="userID">Group owner</param>
        /// <param name="groupModel"></param>
        void Create(int userID, GroupModel groupModel);

        /// <summary>
        /// Creates new group by name
        /// </summary>
        /// <param name="userID">Group owner</param>
        /// <param name="groupName"></param>
        void Create(int userID, string groupName);

        /// <summary>
        /// Gets groupModel by it's ID
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        GroupModel GetGroup(int groupID);

        /// <summary>
        /// Adds user to group by groupID
        /// </summary>
        /// <param name="ownerID">Group ownerID</param>
        /// <param name="userID">User to add</param>
        /// <param name="groupID"></param>
        void AddUser(int ownerID, int userID, int groupID);

        /// <summary>
        /// Adds user to group by groupName
        /// </summary>
        /// <param name="ownerID">Group ownerID</param>
        /// <param name="userID">User to add</param>
        /// <param name="groupName"></param>
        void AddUser(int ownerID, int userID, string groupName);

        /// <summary>
        /// Remove user from group by its ID
        /// </summary>
        /// <param name="ownerID">Group ownerID</param>
        /// <param name="userID">User to add</param>
        /// <param name="groupID"></param>
        void RemoveUser(int ownerID, int userID, int groupID);

        /// <summary>
        /// Remove user from group by its name
        /// </summary>
        /// <param name="ownerID">Group ownerID</param>
        /// <param name="userID">User to add</param>
        /// <param name="groupName"></param>
        void RemoveUser(int ownerID, int userID, string groupName);

        /// <summary>
        /// Delete group by its ID
        /// </summary>
        /// <param name="ownerID">Group ownerID</param>
        /// <param name="groupID"></param>
        void Delete(int ownerID, int groupID);

    }
}
