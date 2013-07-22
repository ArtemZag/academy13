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

        public void Add(string albumName, int ownerID)
        {
            base.Add(new AlbumModel(albumName, ownerID));
        }
    }
}
