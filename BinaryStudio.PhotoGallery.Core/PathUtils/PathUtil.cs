using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    internal class PathUtil : IPathUtil
    {
        private const string DELIMITER = @"\";

        private const string PHOTOS_DIRECTORY_NAME = "photos";
        private const string TEMPORARY_DIRECTORY_NAME = "temporary";
        private const string THUMBNAIL_DIRECTORY_NAME = "thumbnail";

        private readonly string dataVirtualRoot;

        public PathUtil()
        {
            dataVirtualRoot = ConfigurationManager.AppSettings["DataDirectory"];
        }

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
            var builder = new StringBuilder(BuildPhotoDirectoryPath());
            builder.Append(DELIMITER)
                   .Append(userId)
                   .Append(DELIMITER)
                   .Append(albumId);

            return builder.ToString();
        }

        public string BuildOriginalPhotoPath(int userId, int albumId, string photoName, string photoFormat)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(photoName)
                   .Append(photoFormat);

            return builder.ToString();
        }

        public string BuildThumbnailsPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(THUMBNAIL_DIRECTORY_NAME);

            return builder.ToString();
        }

        public IEnumerable<string> BuildTemporaryDirectoriesPaths()
        {
            string photoDirectoryPath = BuildPhotoDirectoryPath();

            IEnumerable<string> usersDirectories = Directory.EnumerateDirectories(photoDirectoryPath);

            var temporaryPhotosDirectories = new Collection<string>();

            foreach (string userDirectory in usersDirectories)
            {
                string temporaryPhotosDirectory = BuildTemporaryDirectoryPath(userDirectory);
                temporaryPhotosDirectories.Add(temporaryPhotosDirectory);
            }

            return temporaryPhotosDirectories;
        }

        public string BuildTemporaryDirectoryPath(int userId)
        {
            var photoDirectoryPath = new StringBuilder(BuildPhotoDirectoryPath());

            photoDirectoryPath.Append(@"\");
            photoDirectoryPath.Append(userId);
            photoDirectoryPath.Append(@"\");
            photoDirectoryPath.Append(TEMPORARY_DIRECTORY_NAME);

            return photoDirectoryPath.ToString();
        }

        private string GetDataDirectory()
        {
            // This method not work for me
            return VirtualPathUtility.ToAbsolute(dataVirtualRoot); 
            
            // And this work good
            //return HostingEnvironment.MapPath(dataVirtualRoot);
        }

        private string BuildTemporaryDirectoryPath(string userDirectoryPath)
        {
            return Path.Combine(userDirectoryPath, TEMPORARY_DIRECTORY_NAME);
        }
    }
}