using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public class AsyncPhotoProcessor
    {
        private static readonly int MinHeight;
        private static readonly int MaxHeight;
        private readonly int albumId;

        private readonly int maxHeight;
        private readonly object syncRoot;

        private readonly int userId;
        private readonly IPathUtil util;

        static AsyncPhotoProcessor()
        {
            MinHeight = 1;
            MaxHeight = 1024;
        }

        public AsyncPhotoProcessor(int userId, int albumId, int maxHeight, IPathUtil util)
        {
            this.maxHeight = maxHeight;

            if (maxHeight < MinHeight)
                throw new ArgumentException(string.Format("Invalid height. Parameter must be greater then {0}",
                    MinHeight));

            if (maxHeight > MaxHeight)
                throw new ArgumentException(string.Format("Invalid height. Parameter must be less or equal then {0}",
                    MaxHeight));

            this.albumId = albumId;
            this.util = util;
            this.userId = userId;
            if (albumId < 0 || userId < 0)
                throw new ArgumentException("Invalid userId and albumId");

            syncRoot = new object();
        }

        public AsyncPhotoProcessor(int userId, IPathUtil util)
        {
            this.util = util;
            this.userId = userId;
            if (userId < 0)
                throw new ArgumentException("Invalid userId and albumId");
        }

        public void SyncOriginalAndThumbnailImages(IEnumerable<PhotoModel> models)
        {
            bool modified = false;
            string pathToThumb = util.BuildAbsoluteThumbnailsDirPath(userId, albumId, maxHeight);

            CreateDirectoriesIfNotExists(pathToThumb);

            Parallel.ForEach(models, new ParallelOptions {MaxDegreeOfParallelism = 1},
                model =>
                {
                    bool changes = SyncCoupleFiles(userId, albumId, model, maxHeight);

                    lock (syncRoot)
                        modified |= changes;
                });

            if (modified)
                DeleteCollages();
        }

        private void CreateDirectoriesIfNotExists(params string[] paths)
        {
            foreach (string path in paths.Where(path => !Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string CreateThumbnail(int userId, int albumId, PhotoModel model, int maxSize)
        {
            string origin = util.BuildAbsoluteOriginalPhotoPath(userId, albumId, model);

            if (File.Exists(origin))
            {
                var thumbnail = util.BuildAbsoluteThumbnailPath(userId, albumId, maxSize, model);
                if (!File.Exists(thumbnail))
                    ThumbnailCreationAction(origin, thumbnail, maxSize, false);

                return util.GetEndUserReference(thumbnail);
            }
            throw new FileNotFoundException(string.Format("File: {0} not found", origin));
        }

        private bool SyncCoupleFiles(int userId, int albumId, PhotoModel model, int maxSize,
            bool isCompressedByTwoBounds = false)
        {
            string origin = util.BuildAbsoluteOriginalPhotoPath(userId, albumId, model);
            string thumbnail = util.BuildAbsoluteThumbnailPath(userId, albumId, maxSize, model);
            if (!File.Exists(origin))
            {
                if (File.Exists(thumbnail))
                {
                    File.Delete(thumbnail);
                    return true;
                }
            }
            else
            {
                if (!File.Exists(thumbnail))
                {
                    ThumbnailCreationAction(origin, thumbnail, maxHeight, isCompressedByTwoBounds);
                    return true;
                }
            }
            return false;
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
            var info = new FileInfo(util.BuildAbsoluteAvatarPath(userId, size));
            if (info.Exists)
                return util.GetEndUserReference(info.FullName);

            var originalInfo = new FileInfo(util.BuildAbsoluteAvatarPath(userId, ImageSize.Original));
            if (originalInfo.Exists)
            {
                string tmpFile = Path.Combine(originalInfo.DirectoryName, MakeRandomFileName("jpg"));
                ThumbnailCreationAction(originalInfo.FullName, tmpFile, (int) size, true);
                File.Move(tmpFile, info.FullName);
                File.Delete(tmpFile);
                return util.GetEndUserReference(info.FullName);
            }
            return util.CustomAvatarPath;
        }

        private string MakeRandomFileName(string ext)
        {
            return string.Format("{0}.{1}", Randomizer.GetString(10), ext);
        }

        private void DeleteCollages()
        {
            string path = util.BuildAbsoluteCollagesDirPath(userId, albumId);

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public string CreateCollageIfNotExist(int width, int rows)
        {
            string path = util.BuildAbsoluteCollagesDirPath(userId, albumId);

            if (Directory.Exists(path))
            {
                string s = ImageFormatHelper.GetImages(path).ToList().First();
                if (s != null)
                    return util.GetEndUserReference(s);
            }

            return MakeCollage(width, rows);
        }

        public IEnumerable<string> GetThumbnails()
        {
            string fullPath = util.BuildAbsoluteThumbnailsDirPath(userId, albumId, maxHeight);

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
            var pathToCollage = util.CreatePathToCollage(userId, albumId);
            string pathToCollages = util.BuildAbsoluteCollagesDirPath(userId, albumId);

            using (Image img = new Bitmap(width, height))
            {
                Graphics grfx = Graphics.FromImage(img);
                SetUpGraphics(grfx);

                TileTheImage(grfx, Randomizer.GetEnumerator(GetThumbnails()), width, height);

                CreateDirectoriesIfNotExists(pathToCollages);
                img.Save(pathToCollage, ImageFormat.Jpeg);
            }
            return util.GetEndUserReference(pathToCollage);
        }

        private string CreatePathToCollage(int userId, int albumId)
        {
            string absolutePhotoDirectoryPath = util.BuildAbsolutePhotoDirectoryPath();

            return Path.Combine(absolutePhotoDirectoryPath, userId.ToString(), albumId.ToString(),
                COLLAGES_DIRECTORY_NAME,
                MakeFileName(Randomizer.GetString(10), "jpg"));
        }

        // todo! do something!
        public string GetEndUserReference(string absolutePath)
        {
            int index = absolutePath.LastIndexOf(@"data\photos");
            return absolutePath.Remove(0, index - 1).Replace(@"\", "/");
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