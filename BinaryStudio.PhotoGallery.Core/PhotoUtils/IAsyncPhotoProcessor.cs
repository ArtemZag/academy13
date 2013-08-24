using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    internal interface IAsyncPhotoProcessor
    {
        bool CreateThumbnailsIfNotExist();

        bool DeleteThumbnailsIfOriginalNotExist();

        void SyncOriginalAndThumbnailImages();

        string CreateCollageIfNotExist(int width, int rows);

        IEnumerable<string> GetThumbnails();

        void SetUpForRandomEnumerable(IEnumerable<string> arr);

        string MakeCollage(int width, int rows);

        IEnumerable<string> GetEnumerator();
    }
}