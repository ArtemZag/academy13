using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class AlbumRepository : BaseRepository<AlbumModel>, IAlbumRepository
    {
        public AlbumRepository(IDatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }
    }
}
