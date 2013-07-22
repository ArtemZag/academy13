using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IAlbumRepository : IBaseRepository<AlbumModel>
    {
        /// <summary>
        /// Adds new album
        /// </summary>
        /// <param name="ownerId">User ID that is album's owner</param>
        /// <returns></returns>
        void Add(string albumName, int ownerId);
    }
}
