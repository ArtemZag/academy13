using System;
using System.Drawing;
using System.IO;
using BinaryStudio.PhotoGallery.Core.PathUtils;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public class PhotoProcessor : IPhotoProcessor
    {
        private readonly IPathUtil _pathUtil;

        public PhotoProcessor(IPathUtil pathUtil)
        {
            _pathUtil = pathUtil;
        }

        /// <summary>
        ///     Creates thumbnails of all formats for original photo
        /// </summary>
        public void CreateThumbnails(int userId, int albumId, int photoId, string format)
        {
            string originalPhotoPath = _pathUtil.BuildAbsoluteOriginalPhotoPath(userId, albumId, photoId, format);

            if (File.Exists(originalPhotoPath))
            {
                CreateThumbnail(userId, albumId, photoId, format, ImageSize.Big);
                CreateThumbnail(userId, albumId, photoId, format, ImageSize.Medium);
                CreateThumbnail(userId, albumId, photoId, format, ImageSize.Small);
            }
        }

        /// <summary>
        ///     Creates thumbnail of all formats for original avatar
        /// </summary>
        public void CreateAvatarThumbnails(int userId)
        {
            string originalAvatarPath = _pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Original);

            if (File.Exists(originalAvatarPath))
            {
                CreateAvatarThumbnail(userId, originalAvatarPath, ImageSize.Big);
                CreateAvatarThumbnail(userId, originalAvatarPath, ImageSize.Medium);
                CreateAvatarThumbnail(userId, originalAvatarPath, ImageSize.Small);
            }
        }

        /// <summary>
        ///     Creates thumbnail for photo with specified format
        /// </summary>
        private void CreateThumbnail(int userId, int albumId, int photoId, string format, ImageSize size)
        {
            string originalPhotoPath = _pathUtil.BuildAbsoluteOriginalPhotoPath(userId, albumId, photoId, format);

            string absoluteThumbnailPath = _pathUtil.BuildAbsoluteThumbailPath(userId, albumId, photoId, format, size);

            Directory.CreateDirectory(Path.GetDirectoryName(absoluteThumbnailPath));

            ThumbnailCreationAction(originalPhotoPath, absoluteThumbnailPath, (int) size);
        }

        private void CreateAvatarThumbnail(int userId, string originalAvatarPath, ImageSize size)
        {
            string path = _pathUtil.BuildAbsoluteAvatarPath(userId, size);

            ThumbnailCreationAction(originalAvatarPath, path, (int) size);
        }

        private void ThumbnailCreationAction(string imagePath, string thumbnailPath, int maxSize)
        {
            using (Image image = Image.FromFile(imagePath))
            {
                Size size = CalculateThumbnailSize(image.Size, maxSize, false);

                using (Image thumb = image.GetThumbnailImage(size.Width, size.Height, () => false, IntPtr.Zero))
                {
                    thumb.Save(thumbnailPath);
                }
            }
        }

        private Size CalculateThumbnailSize(Size size, int maxSize, bool twoBounds)
        {
            if (twoBounds)
            {
                if (size.Width > size.Height)
                    return new Size(maxSize, (int) (((double) size.Height/size.Width)*maxSize));
            }

            return new Size((int) (((double) size.Width/size.Height)*maxSize), maxSize);
        }
    }
}