using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IAuthInfoRepository : IBaseRepository<AuthInfoModel>
    {
        /// <summary>
        /// Adds authentication method for user
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="authProvider">[local][google][facebook]</param>
        void Add(int userID, string authProvider);

        /// <summary>
        /// Adds authentication method for user
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="authProvider">[local][google][facebook]</param>
        /// <param name="authToken">Token for OAuth</param>
        void Add(int userID, string authProvider, string authToken);
    }
}
