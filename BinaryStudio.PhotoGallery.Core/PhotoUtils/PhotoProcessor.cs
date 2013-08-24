using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BinaryStudio.PhotoGallery.Core.PathUtils;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public class PhotoProcessor : IPhotoProcessor
    {
        private readonly IPathUtil pathUtil;

        public PhotoProcessor(IPathUtil pathUtil)
        {
            this.pathUtil = pathUtil;
        }

        public void CreateThumbnails(int userId, int albumId, int photoId, string format)
        {
            string originalPhotoPath = pathUtil.BuildAbsoluteOriginalPhotoPath(userId, albumId, photoId, format);

            if (File.Exists(originalPhotoPath))
            {
                CreateThumbnail(userId, albumId, photoId, format, ImageSize.Big);
                CreateThumbnail(userId, albumId, photoId, format, ImageSize.Medium);
                CreateThumbnail(userId, albumId, photoId, format, ImageSize.Small);
            }
        }

        private void CreateThumbnail(int userId, int albumId, int photoId, string format, ImageSize imageSize)
        {
            string originalPhotoPath = pathUtil.BuildAbsoluteOriginalPhotoPath(userId, albumId, photoId, format);

            string absoluteThumbnailPath = pathUtil.BuildAbsoluteThumbailPath(userId, albumId, photoId, format,
                    imageSize);

            if (!File.Exists(absoluteThumbnailPath))
            {
                ThumbnailCreationAction(originalPhotoPath, absoluteThumbnailPath, (int)imageSize, false);
            }
        }

        public void CreateAvatarThumbnails(int userId)
        {
            string originalAvatarPath = pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Original);

            if (File.Exists(originalAvatarPath))
            {
                CreateSmallAvatar(userId, originalAvatarPath);
                CreateMediumAvatar(userId, originalAvatarPath);
                CreateBigAvatar(userId, originalAvatarPath);
            }
        }

        public IEnumerable<string> GetThumbnails(int userId, int albumId, ImageSize imageSize)
        {
            string thumbnailsDirectoryPath = pathUtil.BuildAbsoluteThumbnailsDirPath(userId, albumId, imageSize);

            return Directory.EnumerateFiles(thumbnailsDirectoryPath);
        }

        private void ThumbnailCreationAction(string imagePath, string thumbnailPath, int maxSize, bool twoBounds)
        {
            using (Image image = Image.FromFile(imagePath))
            {
                Size size = CalculateThumbSize(image.Size, maxSize, twoBounds);

                using (Image thumb = image.GetThumbnailImage(size.Width, size.Height,
                    () => false, IntPtr.Zero))
                {
                    // todo: there are many formats
                    thumb.Save(thumbnailPath, ImageFormat.Jpeg);
                }
            }
        }

        private void CreateBigAvatar(int userId, string originalAvatarPath)
        {
            string path = pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Big);

            ThumbnailCreationAction(originalAvatarPath, path, (int) ImageSize.Big, true);
        }

        private void CreateMediumAvatar(int userId, string originalAvatarPath)
        {
            string path = pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Medium);

            ThumbnailCreationAction(originalAvatarPath, path, (int) ImageSize.Medium, true);
        }

        private void CreateSmallAvatar(int userId, string originalAvatarPath)
        {
            string path = pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Medium);

            ThumbnailCreationAction(originalAvatarPath, path, (int) ImageSize.Medium, true);
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