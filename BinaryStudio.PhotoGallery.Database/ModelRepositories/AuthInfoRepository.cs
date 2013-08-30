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

        public void Add(int userId, string authProvider)
        {
            base.Add(new AuthInfoModel(userId, authProvider));
        }

        public void Add(int userId, string authProvider, string authToken)
        {
            base.Add(new AuthInfoModel(userId, authProvider)
                {
                    AuthProviderId = authToken
                });
        }
    }
}
