using System.Text;
using System.Web;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    internal class UrlUtil : IUrlUtil
    {
        private readonly string siteUrl;

        public UrlUtil()
        {
            siteUrl = HttpContext.Current.Request.Url.Authority;
        }

        public string BuildPhotoViewUrl(int photoId)
        {
            const string PHOTO_PART = "photo";

            return BuildBathByPattern(PHOTO_PART, photoId);
        }

        public string BuildAlbumViewUrl(int albumId)
        {
            const string ALBUM_PART = "album";

            return BuildBathByPattern(ALBUM_PART, albumId);
        }

        public string BuildUserViewUrl(int userId)
        {
            const string USER_PART = "user";

            return BuildBathByPattern(USER_PART, userId);
        }

        private string BuildBathByPattern(string pathPart, int id)
        {
            const string DELIMITER = "/";

            var url = new StringBuilder(/*siteUrl*/);

            url.Append(DELIMITER)
                .Append(pathPart)
                .Append(DELIMITER)
                .Append(id);

            return url.ToString();
        }
    }
}