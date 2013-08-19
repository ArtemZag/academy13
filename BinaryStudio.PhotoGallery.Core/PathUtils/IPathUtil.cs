using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IPathUtil
    {
        /// <returns>Example: ~data\photos</returns>
        string BuildPhotoDirectoryPath();

        /// <returns>Example: ~data\photos\1\1</returns>
        string BuildAlbumPath(int userId, int albumId);

        /// <returns>Example: ~data\photos\1\1\1.png</returns>
        string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string format);

        /// <returns>Example: ~data\photos\1\1\thumbnails</returns>
        string BuildThumbnailsPath(int userId, int albumId);

        /// <returns>Example: ~data\photos\1\avatar.jpg</returns>
        string BuildUserAvatarPath(int userId);

        // todo: change signature
        /// <summary>
        /// Deprecated
        /// </summary>
        string BuildThumbnailPath(int userId, int albumId, int photoId, string format);

        /// <returns>Path in format "C:\\ololo\\ololo"</returns>
        string BuildAbsoluteTemporaryDirectoryPath(int userId);

        /// <returns>Path in format "C:\\ololo\\ololo"</returns>
        string BuildAbsoluteAlbumPath(int userId, int albumId);
    }
}
