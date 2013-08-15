using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IPhotoTagRepository : IBaseRepository<PhotoTagModel>
    {
        /// <summary>
        /// Add tag to photo
        /// </summary>
        /// <param name="tagName">tag name</param>
        void Add(string tagName);
    }
}
