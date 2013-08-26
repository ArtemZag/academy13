using System;
using System.Drawing;
using System.Drawing.Imaging;
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

        private void CreateThumbnail(int userId, int albumId, int photoId, string format, ImageSize imageSize)
        {
            string originalPhotoPath = _pathUtil.BuildAbsoluteOriginalPhotoPath(userId, albumId, photoId, format);

            string absoluteThumbnailPath = _pathUtil.BuildAbsoluteThumbailPath(userId, albumId, photoId, format,
                    imageSize);

            if (!File.Exists(absoluteThumbnailPath))
            {
                ThumbnailCreationAction(originalPhotoPath, absoluteThumbnailPath, (int)imageSize, false);
            }
        }

        public void CreateAvatarThumbnails(int userId)
        {
            string originalAvatarPath = _pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Original);

            if (File.Exists(originalAvatarPath))
            {
                CreateSmallAvatar(userId, originalAvatarPath);
                CreateMediumAvatar(userId, originalAvatarPath);
                CreateBigAvatar(userId, originalAvatarPath);
            }
        }

        private void CreateBigAvatar(int userId, string originalAvatarPath)
        {
            string path = _pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Big);

            ThumbnailCreationAction(originalAvatarPath, path, (int)ImageSize.Big, true);
        }

        private void CreateMediumAvatar(int userId, string originalAvatarPath)
        {
            string path = _pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Medium);

            ThumbnailCreationAction(originalAvatarPath, path, (int)ImageSize.Medium, true);
        }

        private void CreateSmallAvatar(int userId, string originalAvatarPath)
        {
            string path = _pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Medium);

            ThumbnailCreationAction(originalAvatarPath, path, (int)ImageSize.Medium, true);
        }

        private void ThumbnailCreationAction(string imagePath, string thumbnailPath, int maxSize, bool twoBounds)
        {
            using (Image image = Image.FromFile(imagePath))
            {
                Size size = CalculateThumbSize(image.Size, maxSize, twoBounds);

                using (Image thumb = image.GetThumbnailImage(size.Width, size.Height, () => false, IntPtr.Zero))
                {
                    // todo: there are many formats
                    thumb.Save(thumbnailPath);
                }
            }
        }

        private Size CalculateThumbSize(Size size, int maxSize, bool twoBounds)
        {
            if (twoBounds)
            {
                if (size.Width > size.Height)
                    return new Size(maxSize, (int) (((double) size.Height/size.Width) * maxSize));
            }

            return new Size((int) (((double) size.Width/size.Height)*maxSize), maxSize);
        }
    }
}