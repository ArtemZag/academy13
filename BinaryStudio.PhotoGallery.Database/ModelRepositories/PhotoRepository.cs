using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class PhotoRepository : BaseRepository<PhotoModel>, IPhotoRepository
    {
        public PhotoRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }
    }
}
