using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IPathUtil
    {
        string BuildPhotoDirectoryPath();

        string GetAlbumPath(int userId, int albumId);

        string BuildOriginalPhotoPath(int userId, int albumId, string photoName, string photoFormat);

        string BuildThumbnailsPath(int userId, int albumId);

        IEnumerable<string> BuildTemporaryDirectoriesPaths();

        string GetTemporaryDirectoryPath(int userId);
    }
}
