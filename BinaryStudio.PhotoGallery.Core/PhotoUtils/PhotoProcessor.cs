using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.PathUtils;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public class PhotoProcessor
    {
        private readonly int maxHeight;

        private readonly IPathUtil pathUtil;

        public PhotoProcessor(IPathUtil pathUtil)
        {
            this.pathUtil = pathUtil;
        }

        private void CreateDirectoriesIfNotExists(params string[] paths)
        {
            foreach (string path in paths.Where(path => !Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
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

        private void ThumbnailCreationAction(string pathToOriginal, string pathToThumbnail, int maxSize, bool twoBounds)
        {
            using (Image image = Image.FromFile(pathToOriginal))
            {
                Size size = CalculateThumbSize(image.Size, maxSize, twoBounds);

                using (Image thumb = image.GetThumbnailImage(size.Width, size.Height,
                    () => false, IntPtr.Zero))
                {
                    thumb.Save(pathToThumbnail, ImageFormat.Jpeg);
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

        private string MakeRandomFileName()
        {
            return Randomizer.GetString(10);
        }

        public string CreateCollageIfNotExist(int width, int rows)
        {
            string path = pathUtil.BuildAbsoluteCollagesDirPath(userId, albumId);

            if (Directory.Exists(path))
            {
                string s = ImageFormatHelper.GetImages(path).ToList().First();
                if (s != null)
                    return pathUtil.GetEndUserReference(s);
            }

            return MakeCollage(width, rows);
        }

        public IEnumerable<string> GetThumbnails()
        {
            string fullPath = pathUtil.BuildAbsoluteThumbnailsDirPath(userId, albumId, maxHeight);

            if (Directory.Exists(fullPath))
                return ImageFormatHelper.GetImages(fullPath);

            return null;
        }

        private void TileTheImage(Graphics grfx, IEnumerable<string> enumerable, int width, int heigth)
        {
            int iter = 0;
            int sumWidth = 0;
            foreach (string file in enumerable)
            {
                using (Image thumbImage = Image.FromFile(file))
                {
                    grfx.DrawImageUnscaled(thumbImage, sumWidth, iter);
                    sumWidth += thumbImage.Width;
                    if (sumWidth >= width)
                    {
                        sumWidth = 0;
                        iter += maxHeight;
                        if (iter >= heigth)
                            break;
                    }
                }
            }
        }

        private void SetUpGraphics(Graphics grfx)
        {
            grfx.CompositingQuality = CompositingQuality.HighQuality;
            grfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grfx.SmoothingMode = SmoothingMode.HighQuality;
        }

        public string MakeCollage(int width, int rows)
        {
            int height = rows*maxHeight;
            string pathToCollage = CreatePathToCollage(userId, albumId);
            string pathToCollages = pathUtil.BuildAbsoluteCollagesDirPath(userId, albumId);

            using (Image img = new Bitmap(width, height))
            {
                Graphics grfx = Graphics.FromImage(img);
                SetUpGraphics(grfx);

                TileTheImage(grfx, Randomizer.GetEnumerator(GetThumbnails()), width, height);

                CreateDirectoriesIfNotExists(pathToCollages);
                img.Save(pathToCollage, ImageFormat.Jpeg);
            }
            return pathUtil.GetEndUserReference(pathToCollage);
        }

        private string CreatePathToCollage(int userId, int albumId)
        {
            string absolutePhotoDirectoryPath = pathUtil.BuildAbsolutePhotoDirectoryPath();

            return Path.Combine(absolutePhotoDirectoryPath, userId.ToString(), albumId.ToString(),
                COLLAGES_DIRECTORY_NAME,
                MakeFileName(Randomizer.GetString(10), "jpg"));
        }

        // todo! deprecated
        public string MakeFileName(string name, string ext)
        {
            return string.Format("{0}.{1}", name, ext);
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