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

        public void CreateThumbnail(int userId, int albumId, int photoId, string format, ImageSize imageSize)
        {
            string originalPhotoPath = pathUtil.BuildAbsoluteOriginalPhotoPath(userId, albumId, photoId, format);

            if (File.Exists(originalPhotoPath))
            {
                string absoluteThumbnailPath = pathUtil.BuildAbsoluteThumbailPath(userId, albumId, photoId, format, imageSize);

                if (!File.Exists(absoluteThumbnailPath))
                {
                    ThumbnailCreationAction(originalPhotoPath, absoluteThumbnailPath, (int)imageSize, false);
                }
            }
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

        public string GetUserAvatar(ImageSize size)
        {
            var fileInfo = new FileInfo(pathUtil.BuildAbsoluteAvatarPath(userId, size));

            if (fileInfo.Exists)
                return pathUtil.GetEndUserReference(fileInfo.FullName);

            var originalInfo = new FileInfo(pathUtil.BuildAbsoluteAvatarPath(userId, ImageSize.Original));

            if (originalInfo.Exists)
            {
                string tmpFile = Path.Combine(originalInfo.DirectoryName, MakeRandomFileName());

                ThumbnailCreationAction(originalInfo.FullName, tmpFile, (int) size, true);

                File.Move(tmpFile, fileInfo.FullName);
                File.Delete(tmpFile);

                return pathUtil.GetEndUserReference(fileInfo.FullName);
            }

            return pathUtil.CustomAvatarPath;
        }

        public void CreateAvatarThumbnails(int userId)
        {
            throw new NotImplementedException();
        }

        private string MakeRandomFileName()
        {
            return Randomizer.GetString(10);
        }

        public IEnumerable<string> GetThumbnails(int userId, int albumId, ImageSize imageSize)
        {
            string thumbnailsDirectoryPath = pathUtil.BuildAbsoluteThumbnailsDirPath(userId, albumId, imageSize);

            return Directory.EnumerateFiles(thumbnailsDirectoryPath);
        }

        private static Size CalculateThumbSize(Size size, int maxSize, bool twoBounds)
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