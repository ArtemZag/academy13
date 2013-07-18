using BinaryStudio.PhotoGallery.Models;


namespace BinaryStudio.PhotoGallery.Database
{
    class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {

        }
    }
}
