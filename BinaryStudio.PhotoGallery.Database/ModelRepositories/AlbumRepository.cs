using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    internal class AlbumRepository : BaseRepository<AlbumModel>, IAlbumRepository
    {
        public AlbumRepository(DatabaseContext dataBaseContext)
            : base(dataBaseContext)
        {
        }

        public void AddAlbum(int ownerID)
        {
            var album = new AlbumModel {UserModelID = ownerID};
            base.Create(album);
        }
    }
}
