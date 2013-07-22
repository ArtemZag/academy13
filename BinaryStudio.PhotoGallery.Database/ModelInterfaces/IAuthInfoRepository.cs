using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IAuthInfoRepository : IBaseRepository<AuthInfoModel>
    {
        /// <summary>
        /// Adds authentication method for user
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="password">User's password</param>
        /// <param name="authProvider">[local][google][facebook]</param>
        void AddAuthInfo(int userID, string password, string authProvider);

        /// <summary>
        /// Adds authentication method for user
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="password">User's password</param>
        /// <param name="authProvider">[local][google][facebook]</param>
        /// <param name="authToken">Token for OAuth</param>
        void AddAuthInfo(int userID, string password, string authProvider, string authToken);
    }
}
