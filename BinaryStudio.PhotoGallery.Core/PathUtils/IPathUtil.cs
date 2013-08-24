using BinaryStudio.PhotoGallery.Core.PhotoUtils;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IPathUtil
    {
        string CustomAvatarPath { get; }

        /// <returns>Pattern: ~data\photos</returns>
        string BuildPhotoDirectoryPath();

        /// <returns>Pattern: ~data\photos\userId\albumId</returns>
        string BuildAlbumPath(int userId, int albumId);

        /// <returns>Pattern: ~data\photos\userId\albumId\photoId.format</returns>
        string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string format);

        /// <returns>Pattern: ~data\photos\userId\avatar.jpg</returns>
        string BuildAvatarPath(int userId, ImageSize imageSize);

        string BuildPhotoThumbnailPath(int userId, int albumId, int photoId, string format, ImageSize imageSize);

        string BuildAbsoluteUserDirPath(int userId);

        string BuildAbsoluteAvatarPath(int userId, ImageSize imageSize);

        string BuildAbsoluteAlbumPath(int userId, int albumId);

        string BuildAbsoluteThumbnailsDirPath(int userId, int albumId, ImageSize imageSize);

        string BuildAbsoluteCollagesDirPath(int userId, int albumId);

        string BuildAbsoluteOriginalPhotoPath(int userId, int albumId, int photoId, string format);

        string BuildAbsoluteThumbnailPath(int userId, int albumId, int photoId, string format, ImageSize imageSize);
    }
}