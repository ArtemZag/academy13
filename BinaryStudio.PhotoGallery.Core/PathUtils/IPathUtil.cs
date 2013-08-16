using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IPathUtil
    {
        string BuildPhotoDirectoryPath();

        string BuildAlbumPath(int userId, int albumId);

        string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string photoFormat);

        string BuildThumbnailsPath(int userId, int albumId);

        string BuildUserAvatarPath(int userId);

        // todo: delete
        string BuildThumbnailPath(int userId, int albumId, string photoName);

        IEnumerable<string> BuildTemporaryDirectoriesPaths();

        /// <returns>Path in format "C:\\ololo\\ololo"</returns>
        string BuildAbsoluteTemporaryDirectoryPath(int userId);

        /// <returns>Path in format "C:\\ololo\\ololo"</returns>
        string BuildAbsoluteAlbumPath(int userId, int albumId);
    }
}
