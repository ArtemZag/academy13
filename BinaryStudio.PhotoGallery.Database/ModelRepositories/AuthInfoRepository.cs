using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    internal class AuthInfoRepository : BaseRepository<AuthInfoModel>, IAuthInfoRepository
    {
        public AuthInfoRepository(DatabaseContext dataBaseContext)
            : base(dataBaseContext)
        {
        }

        public void Add(int userID, string password, string authProvider)
        {
            var auth = new AuthInfoModel
                {
                    UserModelID = userID,
                    UserPassword = password,
                    AuthProvider = authProvider
                };
            base.Add(auth);
        }

        public void Add(int userID, string password, string authProvider, string authToken)
        {
            var auth = new AuthInfoModel
            {
                UserModelID = userID,
                UserPassword = password,
                AuthProvider = authProvider,
                AuthProviderToken = authToken
            };
            base.Add(auth);
        }
    }
}
