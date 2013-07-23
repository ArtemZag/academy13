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
            base.Add(new AuthInfoModel(userID, password, authProvider));
        }

        public void Add(int userID, string password, string authProvider, string authToken)
        {
            base.Add(new AuthInfoModel(userID, password, authProvider)
                {
                    AuthProviderToken = authToken
                });
        }
    }
}
