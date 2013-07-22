using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IPhotoRepository : IBaseRepository<PhotoModel>
    {
        /// <summary>
        /// Adds photo to album
        /// </summary>
        /// <param name="albumId">Album ID which contains this photo</param>
        /// <param name="ownerId">User ID that post photo</param>
        void Add(int albumId, int ownerId);
    }
}
