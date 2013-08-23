using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    internal class PathUtil : IPathUtil
    {
        private const string DELIMITER = @"\";

        private const string THUMBNAIL_DIRECTORY_NAME = "thumbnails";
        private const string PHOTOS_DIRECTORY_NAME = "photos";

        private readonly string dataVirtualRoot;

        private readonly string appPath;
        private readonly string relativePathToUsersFolder;
        private readonly string usersFolder;
        private readonly string thumbnailsFolderName;
        private readonly string collagesFolderName;
        private readonly string noAvatarPath;
        public PathUtil()
        {
            dataVirtualRoot = ConfigurationManager.AppSettings["DataFolderName"];

            appPath = HttpRuntime.AppDomainAppPath;
            relativePathToUsersFolder = ConfigurationManager.AppSettings["RelativePathToUsersFolder"];
            thumbnailsFolderName = ConfigurationManager.AppSettings["ThumbnailsFolderName"];
            collagesFolderName = ConfigurationManager.AppSettings["CollagesFolderName"];
            noAvatarPath = ConfigurationManager.AppSettings["customAvatarPath"];
            usersFolder = Path.Combine(appPath, relativePathToUsersFolder);
        }

        public string BuildPhotoDirectoryPath()
        {
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

        public string BuildAbsoluteTemporaryAlbumPath(int userId, int albumId)
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

        public string BuildThumbnailPathSized(int userId, int albumId, int photoId, string format,int size)
        {
            var builder = new StringBuilder(BuildThumbnailsPath(userId, albumId));

            builder.Append(DELIMITER).
                Append(size).Append(DELIMITER)
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

        public string BuildPathToUserFolderOnServer(int userId)
        {
            return Path.Combine(usersFolder, userId.ToString());
        }

        public string BuildPathToUserAvatarOnServer(int userId,AvatarSize size = AvatarSize.Original)
        {
            var pathToUserFolder = Path.Combine(usersFolder, userId.ToString());
            switch (size)
            {
                case AvatarSize.Original:
                    return Path.Combine(pathToUserFolder, "avatar.jpg");
                case AvatarSize.Small:
                    return Path.Combine(pathToUserFolder, "smallAvatar.jpg");
                case AvatarSize.Medium:
                    return Path.Combine(pathToUserFolder, "mediumAvatar.jpg");
                case AvatarSize.Big:
                    return Path.Combine(pathToUserFolder, "bigAvatar.jpg");
                default:
                    throw new ArgumentOutOfRangeException("size");
            }
        }

        public string BuildPathToUserAlbumFolderOnServer(int userId, int albumId)
        {
            return Path.Combine(usersFolder, userId.ToString(), albumId.ToString());
        }

        public string BuildPathToUserAlbumThumbnailsFolderOnServer(int userId, int albumId, int thumbnailsSize)
        {
            return Path.Combine(usersFolder,
                                userId.ToString(),
                                albumId.ToString(),
                                thumbnailsFolderName,
                                thumbnailsSize.ToString());
        }

        public string BuildPathToUserAlbumCollagesFolderOnServer(int userId, int albumId)
        {
            return Path.Combine(usersFolder, userId.ToString(), albumId.ToString(), collagesFolderName);
        }

        public string GetEndUserReference(string absolutePath)
        {
            int index = absolutePath.LastIndexOf(relativePathToUsersFolder);
            return absolutePath.Remove(0, index - 1).Replace(@"\", "/");
        }

        public string MakeFileName(string name,string ext)
        {
            return string.Format("{0}.{1}", name, ext);
        }

        public string MakeRandomFileName(string ext)
        {
            return string.Format("{0}.{1}", Randomizer.GetString(10), ext);
        }

        public string BuildPathToOriginalFileOnServer(int userId, int albumId, PhotoModel model)
        {
            return Path.Combine(usersFolder, userId.ToString(), albumId.ToString(), MakeFileName(model.Id.ToString(), model.Format));
        }

        public string BuildPathToThumbnailFileOnServer(int userId, int albumId, int thumbnailsSize, PhotoModel model)
        {
            return Path.Combine(usersFolder, userId.ToString(), albumId.ToString(), thumbnailsFolderName,
                                thumbnailsSize.ToString(), MakeFileName(model.Id.ToString(), "jpg"));
        }

        public string NoAvatar()
        {
            return noAvatarPath;
        }

        public string CreatePathToCollage(int userId, int albumId)
        {
            return Path.Combine(usersFolder, userId.ToString(), albumId.ToString(), collagesFolderName,
                                MakeFileName(Randomizer.GetString(10),"jpg"));
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