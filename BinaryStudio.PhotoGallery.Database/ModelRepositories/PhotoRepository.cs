using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    internal class PhotoRepository : BaseRepository<PhotoModel>, IPhotoRepository
    {
        public PhotoRepository(DatabaseContext dataBaseContext)
            : base(dataBaseContext)
        {
        }

        public void Add(int albumId, int ownerId)
        {
            var photo = new PhotoModel
                {
                    AlbumModelID = albumId,
                    UserModelID = ownerId
                };
            base.Add(photo);
        }
    }
}
