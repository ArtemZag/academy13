using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IPathUtil
    {
        /// <summary>
        ///     Pattern: ~data\photos\userId\[Small|Medium|Big|Original]avatar.jpg
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
        ///     Pattern: ~data\photos\userId\albumId\imageSize\photoId.format
        /// </summary>
        string BuildThumbnailPath(int userId, int albumId, int photoId, string format, ImageSize size);

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId\collage.jpg
        /// </summary>
        string CreateCollagePath(int userId, int albumId);

        string GetCollage(int userId, int albumId);

        string GetUserReference(string absolutePath);

        string BuildCollagePath(int userId, int albumId);

        string BuildAbsoluteAvatarPath(int userId, ImageSize imageSize);

        string BuildAbsoluteAlbumPath(int userId, int albumId);

        string BuildAbsoluteThumbailPath(int userId, int albumId, int photoId, string format, ImageSize size);

        IEnumerable<string> BuildAbsoluteThumbnailsPaths(int userId, int albumId, ImageSize size);

        string BuildAbsoluteCollagePath(int userId, int albumId);

        string BuildAbsoluteCollagesDirPath(int userId, int albumId);

        string BuildAbsoluteOriginalPhotoPath(int userId, int albumId, int photoId, string format);
    }
}