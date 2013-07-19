using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;


namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {

        }
    }
}
