using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IAuthInfoRepository : IBaseRepository<AuthInfoModel>
    {
        /// <summary>
        /// Adds authentication method for user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="authProvider">[local][google][facebook]</param>
        void Add(int userId, string authProvider);

        /// <summary>
        /// Adds authentication method for user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="authProvider">[local][google][facebook]</param>
        /// <param name="authProvideId"></param>
        void Add(int userId, string authProvider, string authProvideId);
    }
}
