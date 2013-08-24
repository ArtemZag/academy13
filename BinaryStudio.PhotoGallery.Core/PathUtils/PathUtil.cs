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

        private const string SUFFIX_AVATAR_FILENAME = "avatar";
        private const string AVATAR_FILE_FORMAT = "jpg";

        // todo: check 
        public string CustomAvatarPath
        {
            get { return CUSTOM_AVATAR_PATH; }
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId
        /// </summary>
        public string BuildAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildUserPath(userId));
            builder.Append(DELIMITER)
                .Append(albumId);

            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId\photoId.format
        /// </summary>
        public string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string format)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                .Append(photoId)
                .Append(MakeExtension(format));

            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId\imageSize\photoId.format
        /// </summary>
        public string BuildThumbnailPath(int userId, int albumId, int photoId, string format, ImageSize imageSize)
        {
            var builder = new StringBuilder(BuildThumbnailsDirPath(userId, albumId, imageSize));

            builder.Append(DELIMITER)
                .Append(photoId)
                .Append(MakeExtension(format));

            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId\[Small|Medium|Big]avatar.jpg
        /// </summary>
        public string BuildAvatarPath(int userId, ImageSize imageSize)
        {
            var builder = new StringBuilder(BuildUserPath(userId));

            builder.Append(imageSize)
                .Append(SUFFIX_AVATAR_FILENAME)
                .Append(MakeExtension(AVATAR_FILE_FORMAT));

            return builder.ToString();
        }

        public string BuildAbsoluteAvatarPath(int userId, ImageSize imageSize)
        {
            return HostingEnvironment.MapPath(BuildAvatarPath(userId, imageSize));
        }

        public string BuildAbsoluteAlbumPath(int userId, int albumId)
        {
            return HostingEnvironment.MapPath(BuildAlbumPath(userId, albumId));
        }

        public string BuildAbsoluteThumbailPath(int userId, int albumId, int photoId, string format, ImageSize imageSize)
        {
            return HostingEnvironment.MapPath(BuildThumbnailPath(userId, albumId, photoId, format, imageSize));
        }

        public string BuildAbsoluteThumbnailsDirPath(int userId, int albumId, ImageSize size)
        {
            return HostingEnvironment.MapPath(BuildThumbnailsDirPath(userId, albumId, size));
        }

        public string BuildAbsoluteCollagesDirPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));

            builder.Append(DELIMITER)
                .Append(COLLAGES_DIRECTORY_NAME);

            string virtualCollagesDirectoryPath = builder.ToString();

            return HostingEnvironment.MapPath(virtualCollagesDirectoryPath);
        }

        public string BuildAbsoluteOriginalPhotoPath(int userId, int albumId, int photoId, string format)
        {
            return HostingEnvironment.MapPath(BuildOriginalPhotoPath(userId, albumId, photoId, format));
        }

        /// <summary>
        ///     Pattern: ~data\photos
        /// </summary>
        private string BuildPhotoDirectoryPath()
        {
            var builder = new StringBuilder();
            builder.Append(GetDataDirectory())
                .Append(DELIMITER)
                .Append(PHOTOS_DIRECTORY_NAME);

            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId\size
        /// </summary>
        private string BuildThumbnailsDirPath(int userId, int albumId, ImageSize size)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));

            builder.Append(DELIMITER)
                .Append((int) size);

            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId
        /// </summary>
        private string BuildUserPath(int userId)
        {
            var builder = new StringBuilder(BuildPhotoDirectoryPath());

            builder.Append(DELIMITER)
                .Append(userId);
            return builder.ToString();
        }

        public string BuildAbsolutePhotoDirectoryPath()
        {
            return HostingEnvironment.MapPath(BuildPhotoDirectoryPath());
        }

        private string MakeExtension(string format)
        {
            return "." + format;
        }

        private string GetDataDirectory()
        {
            return VirtualPathUtility.ToAbsolute(DATA_DIRECTORY_NAME);
        }
    }
}