using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class PhotoCommentRepository : BaseRepository<PhotoCommentModel>, IPhotoCommentRepository
    {
        public PhotoCommentRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }
    }
}
