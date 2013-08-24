using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public interface IPhotoProcessor
    {
        bool CreateThumbnailsIfNotExist();

        bool DeleteThumbnailsIfOriginalNotExist();

        void CreateThumbnail(int userId, int albumId, int photoId, string format, ImageSize imageSize);

        string CreateCollageIfNotExist(int width, int rows);

        IEnumerable<string> GetThumbnails();

        void SetUpForRandomEnumerable(IEnumerable<string> arr);

        string MakeCollage(int width, int rows);

        IEnumerable<string> GetEnumerator();
    }
}