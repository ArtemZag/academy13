using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class PhotoTagRepository : BaseRepository<PhotoTagModel>, IPhotoTagRepository
    {
        public PhotoTagRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }

        public void Add(string tagName)
        {
            base.Add(new PhotoTagModel(tagName));
        }
    }
}
