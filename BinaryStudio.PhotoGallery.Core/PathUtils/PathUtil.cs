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

        private const string THUMBNAILS_DIRECTORY_NAME = "thumbnails";
        private const string PHOTOS_DIRECTORY_NAME = "photos";
        private const string COLLAGES_DIRECTORY_NAME = "collages";
        private const string TEMPORARY_DIRECTORY_NAME = "temporary";

        private const string CUSTOM_AVATAR_PATH = @"~\Content\images\no_avatar.png";

        public string BuildPhotoDirectoryPath()
        {
            var builder = new StringBuilder();
            builder.Append(GetDataDirectory())
                .Append(DELIMITER)
                .Append(PHOTOS_DIRECTORY_NAME);

            return builder.ToString();
        }

        public string BuildAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildUserPath(userId));
            builder.Append(DELIMITER)
                .Append(albumId);

            return builder.ToString();
        }

        public string BuildAbsoluteTemporaryAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                .Append(THUMBNAILS_DIRECTORY_NAME);

            return HostingEnvironment.MapPath(builder.ToString());
        }

        public string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string format)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                .Append(photoId)
                .Append(MakeExtension(format));

            return builder.ToString();
        }

        public string BuildThumbnailsPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                .Append(THUMBNAILS_DIRECTORY_NAME);

            return builder.ToString();
        }

        public string BuildAvatarPath(int userId)
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

        public string BuildThumbnailPath(int userId, int albumId, int photoId, string format)
        {
            var builder = new StringBuilder(BuildThumbnailsPath(userId, albumId));

            builder.Append(DELIMITER)
                .Append(photoId)
                .Append(MakeExtension(format));

            return builder.ToString();
        }

        // todo! error path! 
        public string BuildThumbnailPathSized(int userId, int albumId, int photoId, string format, int size)
        {
            var builder = new StringBuilder(BuildThumbnailsPath(userId, albumId));

            builder.Append(DELIMITER)
                .Append(size)
                .Append(DELIMITER)
                .Append(photoId)
                .Append(MakeExtension(format));

            return builder.ToString();
        }

        public string BuildAbsoluteTemporaryDirectoryPath(int userId)
        {
            string userPath = BuildUserPath(userId);

            string userTempPath = BuildTemporaryDirectoryPath(userPath);

            return HostingEnvironment.MapPath(userTempPath);
        }

        public string BuildAbsoluteUserDirPath(int userId)
        {
            string userVirtualPath = BuildUserPath(userId);

            return HostingEnvironment.MapPath(userVirtualPath);
        }

        // todo! it's need refactoring!
        public string BuildAbsoluteAvatarPath(int userId, ImageSize size = ImageSize.Original)
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
                default:
                    throw new ArgumentOutOfRangeException("size");
            }
        }

        public string BuildAbsoluteAlbumPath(int userId, int albumId)
        {
            string albumVirtualPath = BuildAlbumPath(userId, albumId);

            return HostingEnvironment.MapPath(albumVirtualPath);
        }

        // todo! do something!
        public string BuildAbsoluteThumbnailsDirPath(int userId, int albumId, int thumbnailsSize)
        {
            string absolutePhotoDirectoryPath = BuildAbsolutePhotoDirectoryPath();

            return Path.Combine(absolutePhotoDirectoryPath, userId.ToString(),
                albumId.ToString(),
                THUMBNAILS_DIRECTORY_NAME,
                thumbnailsSize.ToString());
        }

        public string BuildAbsoluteAlbumCollagesDirPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));

            builder.Append(DELIMITER)
                .Append(COLLAGES_DIRECTORY_NAME);

            string virtualCollagesDirectoryPath = builder.ToString();

            return HostingEnvironment.MapPath(virtualCollagesDirectoryPath);
        }

        // todo! do something!
        public string GetEndUserReference(string absolutePath)
        {
            int index = absolutePath.LastIndexOf(@"data\photos");
            return absolutePath.Remove(0, index - 1).Replace(@"\", "/");
        }

        // todo! deprecated
        public string MakeFileName(string name, string ext)
        {
            return string.Format("{0}.{1}", name, ext);
        }

        public string CustomAvatarPath
        {
            get { return CUSTOM_AVATAR_PATH; }
        }

        // todo! do something!
        public string BuildPathToOriginalFileOnServer(int userId, int albumId, int photoId, string format)
        {
            string absolutePhotoDirectoryPath = BuildAbsolutePhotoDirectoryPath();

            return Path.Combine(absolutePhotoDirectoryPath, userId.ToString(), albumId.ToString(),
                MakeFileName(photoId.ToString(), format));
        }

        // todo! do something!
        public string BuildPathToThumbnailFileOnServer(int userId, int albumId, int photoId, string format,
            int thumbnailsSize)
        {
            string absolutePhotoDirectoryPath = BuildAbsolutePhotoDirectoryPath();

            return Path.Combine(absolutePhotoDirectoryPath, userId.ToString(), albumId.ToString(),
                THUMBNAILS_DIRECTORY_NAME,
                thumbnailsSize.ToString(), MakeFileName(photoId.ToString(), format));
        }

        // todo! do something!
        public string CreatePathToCollage(int userId, int albumId)
        {
            string absolutePhotoDirectoryPath = BuildAbsolutePhotoDirectoryPath();

            return Path.Combine(absolutePhotoDirectoryPath, userId.ToString(), albumId.ToString(),
                COLLAGES_DIRECTORY_NAME,
                MakeFileName(Randomizer.GetString(10), "jpg"));
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

        private string BuildTemporaryDirectoryPath(string userDirectoryPath)
        {
            return Path.Combine(userDirectoryPath, TEMPORARY_DIRECTORY_NAME);
        }
    }
}