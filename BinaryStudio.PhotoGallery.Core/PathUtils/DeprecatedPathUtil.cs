using System.IO;
using System.Text;
using System.Web;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    /// <summary>
    /// Deprecated
    /// </summary>
    public static class DeprecatedPathUtil
    {
        private const string DELIMITER = @"/";

        private const string PHOTOS_DIRECTORY_NAME = "Photos";
        private const string TEMPORARY_DIRECTORY_NAME = "temporary";
        private const string THUMBNAIL_DIRECTORY_NAME = "thumbnail";

        public static string ContentDir
        {
            get
            {
                const string CONTENT_VIRTUAL_ROOT = "~/Content";

                return VirtualPathUtility.ToAbsolute(CONTENT_VIRTUAL_ROOT);
            }
        }

        public static string ImageDir
        {
            get { return string.Format("{0}/{1}", ContentDir, "Images"); }
        }

        public static string CssDir
        {
            get { return string.Format("{0}/{1}", ContentDir, "Css"); }
        }

        public static string ImageUrl(string imageFile)
        {
            string result = string.Format("{0}/{1}", ImageDir, imageFile);
            return result;
        }

        public static string CssUrl(string cssFile)
        {
            string result = string.Format("{0}/{1}", CssDir, cssFile);
            return result;
        }

        /// <summary>
        /// Returns path to directory (App/Content/Photos) that contains users directories.
        /// </summary>
        public static string BuildPhotoDirectoryPath()
        {
            var builder = new StringBuilder();
            builder.Append(ImageDir)
                   .Append(DELIMITER)
                   .Append(PHOTOS_DIRECTORY_NAME);

            return builder.ToString();
        }

        public static string BuildAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildPhotoDirectoryPath());
            builder.Append(DELIMITER)
                   .Append(userId)
                   .Append(DELIMITER)
                   .Append(albumId);

            return builder.ToString();
        }

        public static string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string photoFormat)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(photoId)
                   .Append(photoFormat);

            return builder.ToString();
        }

        public static string BuildThumbnailsPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(THUMBNAIL_DIRECTORY_NAME);

            return builder.ToString();
        }

        public static string BuildTemporaryDirectoryPath(string userDirectoryPath)
        {
            return Path.Combine(userDirectoryPath, TEMPORARY_DIRECTORY_NAME);
        }
    }
}