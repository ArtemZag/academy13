using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public class AsyncPhotoProcessor
    {
        private static readonly int MinHeight;
        private static readonly int MaxHeight;

        private readonly IPathUtil util;
        private readonly object syncRoot;
        private readonly int maxHeight;
        private readonly int userId;
        private readonly int albumId;
        
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
            var modified = false;
            var pathToThumb = util.BuildPathToUserAlbumThumbnailsFolderOnServer(userId, albumId, maxHeight);
            
            CreateDirectoriesIfNotExists(pathToThumb);

            Parallel.ForEach(models, new ParallelOptions {MaxDegreeOfParallelism = 1},
                             model =>
                                 {
                                     var changes = SyncCoupleFiles(userId, albumId, model, maxHeight);   

                                     lock (syncRoot)
                                         modified |= changes;
                                 });

            if (modified)
                DeleteCollages();
        }

        private void CreateDirectoriesIfNotExists(params string[] paths)
        {
            foreach (var path in paths.Where(path => !Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string CreateThumbnail(int userId, int albumId, PhotoModel model, int maxSize)
        {
            var origin = util.BuildPathToOriginalFileOnServer(userId, albumId, model);
            
            if (File.Exists(origin))
            {
                var thumbnail = util.BuildPathToThumbnailFileOnServer(userId, albumId, maxSize, model);
                if (!File.Exists(thumbnail))
                    ThumbnailCreationAction(origin, thumbnail, maxSize, false);
                
                return util.GetEndUserReference(thumbnail);
            }
            throw new FileNotFoundException(string.Format("File: {0} not found", origin));
        }

        private bool SyncCoupleFiles(int userId,int albumId, PhotoModel model, int maxSize,
                                     bool isCompressedByTwoBounds=false)
        {
            string origin = util.BuildPathToOriginalFileOnServer(userId, albumId, model);
            string thumbnail = util.BuildPathToThumbnailFileOnServer(userId, albumId, maxSize, model);
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
            using (var image = Image.FromFile(pathToOriginal))
            {
                var size = CalculateThumbSize(image.Size, maxSize, twoBounds);
                using (var thumb = image.GetThumbnailImage(size.Width, size.Height,
                                                           () => false, IntPtr.Zero))
                {
                    thumb.Save(pathToThumbnail, ImageFormat.Jpeg);
                }
            }
        }

        public string GetUserAvatar(AvatarSize size)
        {
            var info = new FileInfo(util.BuildPathToUserAvatarOnServer(userId, size));
            if (info.Exists)
                return util.GetEndUserReference(info.FullName);

            var originalInfo = new FileInfo(util.BuildPathToUserAvatarOnServer(userId, AvatarSize.Original));
            if (originalInfo.Exists)
            {
                var tmpFile = Path.Combine(originalInfo.DirectoryName, util.MakeRandomFileName("jpg"));
                ThumbnailCreationAction(originalInfo.FullName, tmpFile, (int) size, true);
                File.Move(tmpFile, info.FullName);
                File.Delete(tmpFile);
                return util.GetEndUserReference(info.FullName);
            }
            return util.NoAvatar();
        }

        private void DeleteCollages()
        {
            string path = util.BuildPathToUserAlbumCollagesFolderOnServer(userId, albumId);

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public string CreateCollageIfNotExist(int width, int rows)
        {
            string path = util.BuildPathToUserAlbumCollagesFolderOnServer(userId, albumId);

            if (Directory.Exists(path))
            {
                var s = ImageFormatHelper.GetImages(path).ToList().First();
                if (s != null)
                    return util.GetEndUserReference(s);
            }

            return MakeCollage(width, rows);
        }

        public IEnumerable<string> GetThumbnails()
        {
            var fullPath = util.BuildPathToUserAlbumThumbnailsFolderOnServer(userId, albumId, maxHeight);
            
            if (Directory.Exists(fullPath))
                return ImageFormatHelper.GetImages(fullPath);

            return null;
        }

        private void TileTheImage(Graphics grfx, IEnumerable<string> enumerable, int width, int heigth)
        {
            var iter = 0;
            var sumWidth = 0;
            foreach (var file in enumerable)
            {
                using (var thumbImage = Image.FromFile(file))
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
            var height = rows * maxHeight;
            var pathToCollage = util.CreatePathToCollage(userId, albumId);
            var pathToCollages = util.BuildPathToUserAlbumCollagesFolderOnServer(userId, albumId);

            using (Image img = new Bitmap(width, height))
            {
                var grfx = Graphics.FromImage(img);
                SetUpGraphics(grfx);

                TileTheImage(grfx, Randomizer.GetEnumerator(GetThumbnails()), width, height);

                CreateDirectoriesIfNotExists(pathToCollages);
                img.Save(pathToCollage, ImageFormat.Jpeg);
            }
            return util.GetEndUserReference(pathToCollage);
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