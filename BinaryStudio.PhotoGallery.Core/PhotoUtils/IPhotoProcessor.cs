using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public interface IPhotoProcessor
    {
        void CreateThumbnail(int userId, int albumId, int photoId, string format, ImageSize imageSize);

        void CreateAvatarThumbnails(int userId);

        IEnumerable<string> GetThumbnails(int userId, int albumId, ImageSize imageSize);
    }
}