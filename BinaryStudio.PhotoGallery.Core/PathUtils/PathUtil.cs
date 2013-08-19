using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    internal class PathUtil : IPathUtil
    {
        private const string DELIMITER = @"\";

        private const string THUMBNAIL_DIRECTORY_NAME = "thumbnails";

        private readonly string dataVirtualRoot;

        private static readonly string appPath;
        private static readonly string dataFolderName;
        private static readonly string photosFolderName;
        private static readonly string usersFolder;
        private static readonly string avatarFileName;
        private static readonly string thumbnailsFolderName;
        private static readonly string collagesFolderName;

        public PathUtil()
        {
            dataVirtualRoot = ConfigurationManager.AppSettings["DataDirectory"]; 
        }

        static PathUtil()
        {
            appPath = HttpRuntime.AppDomainAppPath;
            dataFolderName = ConfigurationManager.AppSettings["dataFolderName"];
            photosFolderName = ConfigurationManager.AppSettings["photosFolderName"];
            avatarFileName = ConfigurationManager.AppSettings["AvatarFileName"];
            thumbnailsFolderName = ConfigurationManager.AppSettings["ThumbnailsFolderName"];
            collagesFolderName = ConfigurationManager.AppSettings["CollagesFolderName"];
            usersFolder = BuildPathToUsersFolderOnServer();
        }

        public string BuildPhotoDirectoryPath()
        {
            const string PHOTOS_DIRECTORY_NAME = "photos";

            var builder = new StringBuilder();
            builder.Append(GetDataDirectory())
                   .Append(DELIMITER)
                   .Append(PHOTOS_DIRECTORY_NAME);

            return builder.ToString();
        }

        public string BuildUserPath(int userId)
        {
            var builder = new StringBuilder(BuildPhotoDirectoryPath());
            builder.Append(DELIMITER)
                   .Append(userId);
            return builder.ToString();
        }

        public string BuildAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildUserPath(userId));
            builder.Append(DELIMITER)
                   .Append(albumId);

            return builder.ToString();
        }

        public string BuildAbsoluteAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(THUMBNAIL_DIRECTORY_NAME);
            
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
                   .Append(THUMBNAIL_DIRECTORY_NAME);

            return builder.ToString();
        }

        public string BuildUserAvatarPath(int userId)
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

        private string MakeExtension(string format)
        {
            return "." + format;
        }

        public string BuildAbsoluteTemporaryDirectoryPath(int userId)
        {
            var userPath = BuildUserPath(userId);

            var userTempPath = BuildTemporaryDirectoryPath(userPath);

            return HostingEnvironment.MapPath(userTempPath);
        }

        private static string BuildPathToUsersFolderOnServer()
        {
            var stringBuilder = new StringBuilder(appPath);
            return stringBuilder.Append(dataFolderName)
                                .Append(DELIMITER)
                                .Append(photosFolderName).ToString();
        }

        public static string BuildPathToUserFolderOnServer(int userId)
        {
            var stringBuilder = new StringBuilder(usersFolder);
            return stringBuilder.Append(DELIMITER).
                                 Append(userId).ToString();
        }

        public static string BuildPathToUserAvatarOnServer(int userId)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserFolderOnServer(userId));
            return stringBuilder.Append(DELIMITER).
                                 Append(avatarFileName).ToString();
        }

        public static string BuildPathToUserAlbumFolderOnServer(int userId, int albumId)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserFolderOnServer(userId));
            return stringBuilder.Append(DELIMITER).
                                 Append(albumId).ToString();
        }

        public static string BuildPathToUserAlbumThumbnailsFolderOnServer(int userId, int albumId, int thumbnailsSize)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserAlbumFolderOnServer(userId, albumId));
            return stringBuilder.Append(DELIMITER).
                                 Append(thumbnailsFolderName)
                                .Append(DELIMITER)
                                .Append(thumbnailsSize).ToString();
        }

        public static string BuildPathToUserAlbumCollagesFolderOnServer(int userId, int albumId)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserAlbumFolderOnServer(userId, albumId));
            return stringBuilder.Append(DELIMITER).
                                 Append(collagesFolderName).ToString();
        }

        private string GetDataDirectory()
        {
            return VirtualPathUtility.ToAbsolute(dataVirtualRoot); 
        }

        private string GetCustomAvatar()
        {
            const string CUSTOM_AVATAR_PATH = @"~\Content\images\no_avatar.png";

            return VirtualPathUtility.ToAbsolute(CUSTOM_AVATAR_PATH);
        }

        private string BuildTemporaryDirectoryPath(string userDirectoryPath)
        {
            const string TEMPORARY_DIRECTORY_NAME = "temporary";

            return Path.Combine(userDirectoryPath, TEMPORARY_DIRECTORY_NAME);
        }
    }
}