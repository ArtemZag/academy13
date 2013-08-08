using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IPathUtil
    {
        string BuildPhotoDirectoryPath();

        string BuildAlbumPath(int userId, int albumId);

        string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string photoFormat);

        string BuildThumbnailsPath(int userId, int albumId);

        IEnumerable<string> BuildTemporaryDirectoriesPaths();

        string GetAbsoluteRoot();
    }
}
