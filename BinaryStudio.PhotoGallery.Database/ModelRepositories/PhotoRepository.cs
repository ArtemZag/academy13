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

        public void Add(int albumID, int ownerID)
        {
            var photo = new PhotoModel
                {
                    AlbumModelID = albumID,
                    UserModelID = ownerID
                };
            base.Add(photo);
        }
    }
}
