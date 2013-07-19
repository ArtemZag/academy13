using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class AuthInfoRepository : BaseRepository<AuthInfoModel>, IAuthInfoRepository
    {
        public AuthInfoRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }
    }
}
