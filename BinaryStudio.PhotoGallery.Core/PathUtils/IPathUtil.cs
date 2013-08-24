using BinaryStudio.PhotoGallery.Core.PhotoUtils;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IPathUtil
    {
        string CustomAvatarPath { get; }

        /// <summary>
        ///     Pattern: ~data\photos\userId\[Small|Medium|Big]avatar.jpg
        /// </summary>
        string BuildAvatarPath(int userId, ImageSize imageSize);

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId
        /// </summary>
        string BuildAlbumPath(int userId, int albumId);

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId\photoId.format
        /// </summary>
        string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string format);

        /// <summary>
        ///     Pattern: ~data\photos\userId\avatar.jpg
        /// </summary>
        string BuildOriginalAvatarPath(int userId);

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId\imageSize\photoId.format
        /// </summary>
        string BuildThumbnailPath(int userId, int albumId, int photoId, string format, ImageSize imageSize);

        string BuildAbsoluteAvatarPath(int userId, ImageSize imageSize);

        string BuildAbsoluteAlbumPath(int userId, int albumId);

        string BuildAbsoluteThumbailPath(int userId, int albumId, int photoId, string format, ImageSize imageSize);

        string BuildAbsoluteThumbnailsDirPath(int userId, int albumId, ImageSize size);

        string BuildAbsoluteCollagesDirPath(int userId, int albumId);

        string BuildAbsoluteOriginalPhotoPath(int userId, int albumId, int photoId, string format);
    }
}