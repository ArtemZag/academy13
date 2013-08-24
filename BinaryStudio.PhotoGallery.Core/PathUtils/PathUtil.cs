using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    internal class PathUtil : IPathUtil
    {
        private const string DELIMITER = @"\";

        private const string DATA_DIRECTORY_NAME = @"~\data";

        private const string PHOTOS_DIRECTORY_NAME = "photos";
        private const string COLLAGES_DIRECTORY_NAME = "collages";

        private const string CUSTOM_AVATAR_PATH = @"~\Content\images\no_avatar.png";

        /// <returns>Pattern: ~data\photos</returns>
        public string BuildPhotoDirectoryPath()
        {
            var builder = new StringBuilder();
            builder.Append(GetDataDirectory())
                .Append(DELIMITER)
                .Append(PHOTOS_DIRECTORY_NAME);

            return builder.ToString();
        }

        /// <returns>Pattern: ~data\photos\userId\albumId</returns>
        public string BuildAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildUserPath(userId));
            builder.Append(DELIMITER)
                .Append(albumId);

            return builder.ToString();
        }

        /// <returns>Pattern: ~data\photos\userId\albumId\photoId.format</returns>
        public string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string format)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                .Append(photoId)
                .Append(MakeExtension(format));

            return builder.ToString();
        }

        /// <returns>Pattern: ~data\photos\userId\albumId\imageSize\photoId.format</returns>
        public string BuildPhotoThumbnailPath(int userId, int albumId, int photoId, string format, ImageSize imageSize)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));

            builder.Append(DELIMITER)
                .Append((int)imageSize)
                .Append(DELIMITER)
                .Append(photoId)
                .Append(MakeExtension(format));

            return builder.ToString();
        }

        public string BuildAvatarPath(int userId, ImageSize imageSize)
        {
            const string AVATAR_FILE_NAME = "avatar.jpg";

            var builder = new StringBuilder(BuildUserPath(userId));
            builder.Append(DELIMITER)
                .Append(AVATAR_FILE_NAME);

            if (!File.Exists(HostingEnvironment.MapPath(builder.ToString())))
            {
                builder.Clear();

                builder.Append(GetCustomAvatar());
            }

            return builder.ToString();
        }

        public string BuildAbsoluteUserDirPath(int userId)
        {
            string userVirtualPath = BuildUserPath(userId);

            return HostingEnvironment.MapPath(userVirtualPath);
        }

        // todo! it's need refactoring!
        public string BuildAbsoluteAvatarPath(int userId, ImageSize size)
        {
            string userDirectoryPath = BuildAbsoluteUserDirPath(userId);

            switch (size)
            {
                case ImageSize.Original:
                    return Path.Combine(userDirectoryPath, "avatar.jpg");
                case ImageSize.Small:
                    return Path.Combine(userDirectoryPath, "smallAvatar.jpg");
                case ImageSize.Medium:
                    return Path.Combine(userDirectoryPath, "mediumAvatar.jpg");
                case ImageSize.Big:
                    return Path.Combine(userDirectoryPath, "bigAvatar.jpg");
            }
        }

        public string BuildAbsoluteAlbumPath(int userId, int albumId)
        {
            string albumVirtualPath = BuildAlbumPath(userId, albumId);

            return HostingEnvironment.MapPath(albumVirtualPath);
        }

        // todo! do something!
        public string BuildAbsoluteThumbnailsDirPath(int userId, int albumId, ImageSize imageSize)
        {
            string absolutePhotoDirectoryPath = BuildAbsolutePhotoDirectoryPath();

            return Path.Combine(absolutePhotoDirectoryPath, userId.ToString(),
                albumId.ToString(),
                THUMBNAILS_DIRECTORY_NAME,
                thumbnailsSize.ToString());
        }

        public string BuildAbsoluteCollagesDirPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));

            builder.Append(DELIMITER)
                .Append(COLLAGES_DIRECTORY_NAME);

            string virtualCollagesDirectoryPath = builder.ToString();

            return HostingEnvironment.MapPath(virtualCollagesDirectoryPath);
        }

        public string CustomAvatarPath
        {
            get { return CUSTOM_AVATAR_PATH; }
        }

        public string BuildAbsoluteOriginalPhotoPath(int userId, int albumId, int photoId, string format)
        {
            string virtualOriginalPhotoPath = BuildOriginalPhotoPath(userId, albumId, photoId, format);

            return HostingEnvironment.MapPath(virtualOriginalPhotoPath);
        }

        public string BuildAbsoluteThumbnailPath(int userId, int albumId, int photoId, string format, ImageSize imageSize)
        {
            throw new NotImplementedException();
        }

        public string BuildUserPath(int userId)
        {
            var builder = new StringBuilder(BuildPhotoDirectoryPath());
            builder.Append(DELIMITER)
                .Append(userId);
            return builder.ToString();
        }

        public string BuildAbsolutePhotoDirectoryPath()
        {
            string virtualPhotoDirectoryPath = BuildPhotoDirectoryPath();

            return HostingEnvironment.MapPath(virtualPhotoDirectoryPath);
        }

        private string MakeExtension(string format)
        {
            return "." + format;
        }

        private string GetDataDirectory()
        {
            return VirtualPathUtility.ToAbsolute(DATA_DIRECTORY_NAME);
        }

        private string GetCustomAvatar()
        {
            return VirtualPathUtility.ToAbsolute(CUSTOM_AVATAR_PATH);
        }
    }
}