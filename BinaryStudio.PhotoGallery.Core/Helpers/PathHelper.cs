﻿using System.Text;
using System.Web;

namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public static class PathHelper
    {
        private const string DELIMITER = "//";

        private const string PHOTOS_DIRECTORY_NAME = "photos";
        private const string THUMBNAIL_DIRECTORY_NAME = "thumbnail";
        private const string COLLAGES_DIRECTORY_NAME = "collages";

        public static string ContentDir
        {
            get
            {
                const string contentVirtualRoot = "~/Content";
                return VirtualPathUtility.ToAbsolute(contentVirtualRoot);
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

        public static string GetAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder();
            builder.Append(PHOTOS_DIRECTORY_NAME)
                   .Append(DELIMITER)
                   .Append(userId)
                   .Append(DELIMITER)
                   .Append(albumId);

            return builder.ToString();
        }

        public static string GetOriginalPhotoPath(int userId, int albumId, string name)
        {
            var builder = new StringBuilder(GetAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(name);
            // todo: what about extension? .jpg, .png etc. 

            return builder.ToString();
        }

        public static string GetThumbnailPath(int userId, int albumId)
        {
            var builder = new StringBuilder(GetAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(THUMBNAIL_DIRECTORY_NAME);

            return builder.ToString();
        }

        public static string GetCollagesPath(int userId, int albumId)
        {
            var builder = new StringBuilder(GetAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(COLLAGES_DIRECTORY_NAME);

            return builder.ToString();
        }
    }
}