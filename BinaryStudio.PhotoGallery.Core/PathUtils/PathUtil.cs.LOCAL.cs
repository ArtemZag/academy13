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

        private readonly string appPath;
        private readonly string dataFolderName;
        private readonly string photosFolderName;
        private readonly string usersFolder;
        private readonly string avatarFileName;
        private readonly string thumbnailsFolderName;
        private readonly string collagesFolderName;
        private readonly string noAvatarPath;
        private readonly string thumbExtension;
        public PathUtil()
        {
            dataVirtualRoot = ConfigurationManager.AppSettings["DataDirectory"];

            appPath = HttpRuntime.AppDomainAppPath;
            dataFolderName = ConfigurationManager.AppSettings["dataFolderName"];
            photosFolderName = ConfigurationManager.AppSettings["photosFolderName"];
            avatarFileName = ConfigurationManager.AppSettings["AvatarFileName"];
            thumbnailsFolderName = ConfigurationManager.AppSettings["ThumbnailsFolderName"];
            collagesFolderName = ConfigurationManager.AppSettings["CollagesFolderName"];
            noAvatarPath = ConfigurationManager.AppSettings["customAvatarPath"];
            thumbExtension = ConfigurationManager.AppSettings["ThumbnailExtension"];
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

        public string BuildPathToUsersFolderOnServer()
        {
            var stringBuilder = new StringBuilder(appPath);
            return stringBuilder.Append(dataFolderName)
                                .Append(DELIMITER)
                                .Append(photosFolderName).ToString();
        }

        public string BuildPathToUserFolderOnServer(int userId)
        {
            var stringBuilder = new StringBuilder(usersFolder);
            return stringBuilder.Append(DELIMITER).
                                 Append(userId).ToString();
        }

        public string BuildPathToUserAvatarOnServer(int userId)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserFolderOnServer(userId));
            return stringBuilder.Append(DELIMITER).
                                 Append(avatarFileName).ToString();
        }

        public string BuildPathToUserAlbumFolderOnServer(int userId, int albumId)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserFolderOnServer(userId));
            return stringBuilder.Append(DELIMITER).
                                 Append(albumId).ToString();
        }

        public string BuildPathToUserAlbumThumbnailsFolderOnServer(int userId, int albumId, int thumbnailsSize)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserAlbumFolderOnServer(userId, albumId));
            return stringBuilder.Append(DELIMITER).
                                 Append(thumbnailsFolderName)
                                .Append(DELIMITER)
                                .Append(thumbnailsSize).ToString();
        }

        public string BuildPathToUserAlbumCollagesFolderOnServer(int userId, int albumId)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserAlbumFolderOnServer(userId, albumId));
            return stringBuilder.Append(DELIMITER).
                                 Append(collagesFolderName).ToString();
        }

        public string GetEndUserReference(string absolutePath)
        {
            int index = absolutePath.LastIndexOf(dataFolderName);
            return absolutePath.Remove(0, index - 1).Replace(@"\", "/");
        }

        public string MakeFileNameWithExtension(string name)
        {
            return string.Format(@"{0}{1}", name, thumbExtension);
        }

        public string BuildPathToOriginalFileOnServer(int userId, int albumId, string originalName)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserAlbumFolderOnServer(userId, albumId));
            return stringBuilder.Append(DELIMITER).
                                 Append(originalName).ToString();
        }

        public string BuildPathToThumbnailFileOnServer(int userId, int albumId, int thumbnailsSize, string originalPath)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserAlbumThumbnailsFolderOnServer(userId,albumId,thumbnailsSize));
            return stringBuilder.Append(DELIMITER).
                                 Append(MakeFileNameWithExtension(Path.GetFileNameWithoutExtension(originalPath)))
                                .ToString();
        }

        public string NoAvatar()
        {
            return noAvatarPath;
        }

        public string MakePathToCollage(int userId, int albumId, int lenght)
        {
            var stringBuilder = new StringBuilder(BuildPathToUserAlbumCollagesFolderOnServer(userId, albumId));
            return stringBuilder.Append(DELIMITER)
                                .Append(MakeFileNameWithExtension(Randomizer.GetString(lenght)))
                                .ToString();
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