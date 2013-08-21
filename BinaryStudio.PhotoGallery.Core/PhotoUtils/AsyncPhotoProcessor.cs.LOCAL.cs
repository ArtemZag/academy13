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

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public class AsyncPhotoProcessor
    {
        private static readonly int MinHeight;
        private static readonly int MaxHeight;
        private static readonly Random rnd;
        private static readonly int avatarMaxSize;
        private static readonly int MaxAvatarBound;

        private readonly IPathUtil util;
        private readonly object syncRoot;
        private readonly int maxHeight;
        private readonly int userId;
        private readonly int albumId;
        
        static AsyncPhotoProcessor()
        {
            MinHeight = 1;
            MaxHeight = 1024;
            MaxAvatarBound = 96;
            //KB
            avatarMaxSize = 1024*8;
            rnd = new Random((int)DateTime.Now.Ticks);
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

        public void SyncOriginalAndThumbnailImages()
        {
            bool modified,deleted,created;
            modified = deleted = created = false;

            var pathToThumb = util.BuildPathToUserAlbumThumbnailsFolderOnServer(userId, albumId, maxHeight);
            var userAlbumPath = util.BuildPathToUserAlbumFolderOnServer(userId, albumId);

            CreateDirectoriesIfNotExists(pathToThumb);
            var files = ImageFormatHelper.GetImages(userAlbumPath);

            Parallel.ForEach(files, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount},
                             path =>
                                 {
                                     var pathToThumbnailOfImage =
                                         util.BuildPathToThumbnailFileOnServer(userId, albumId, maxHeight, path);
                                     
                                     deleted = DeleteThumbnailIfOriginalNotExists(path, pathToThumbnailOfImage);
                                     if (!deleted)
                                         created = CreateThumbnailIfNotExists(path, pathToThumbnailOfImage, maxHeight);

                                     lock (syncRoot)
                                         modified |= deleted | created;
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

        private bool CreateThumbnailIfNotExists(string pathToOriginal, string pathToThumbnail,int maxSize)
        {
            if (!File.Exists(pathToThumbnail))
            {
                ThumbnailCreationAction(pathToOriginal, pathToThumbnail, maxSize, false);
                return true;
            }
            return false;
        }

        private void ThumbnailCreationAction(string pathToOriginal, string pathToThumbnail,int maxSize,bool twoBounds=true)
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

        private bool DeleteThumbnailIfOriginalNotExists(string pathToOriginal, string pathToThumbnail)
        {
            if (!File.Exists(pathToOriginal))
            {
                File.Delete(pathToThumbnail);
                return true;
            }
            return false;
        }

        public string GetUserAvatar()
        {
            var path = util.BuildPathToUserAvatarOnServer(userId);
            var info = new FileInfo(path);
            if (info.Exists)
            {
                if (info.Length < avatarMaxSize)
                {
                    return util.GetEndUserReference(info.FullName);
                }
                else
                {
                    var s = util.BuildPathToUserFolderOnServer(userId);
                    s = Path.Combine(s,util.MakeFileNameWithExtension(Randomizer.GetString(15)));
                    ThumbnailCreationAction(info.FullName, s, MaxAvatarBound);
                    var newThumb = new FileInfo(s);
                    newThumb.Replace(info.FullName, Path.GetDirectoryName(newThumb.FullName) + @"\avatar_big.jpg");
                    return util.BuildPathToUserAvatarOnServer(userId);
                }
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
                return util.GetEndUserReference(ImageFormatHelper.GetImages(path).ToList().First());

            return util.GetEndUserReference(MakeCollage(width, rows));
        }

        private IEnumerable<string> GetThumbnails()
        {
            string fullPath = util.BuildPathToUserAlbumThumbnailsFolderOnServer(userId, albumId, maxHeight);
            
            if (Directory.Exists(fullPath))
                return ImageFormatHelper.GetImages(fullPath);

            return null;
        }

        private void TileTheImage(Graphics grfx, IEnumerable<string> enumerable, int width, int heigth)
        {
            int iter = 0;
            int sumWidth = 0;
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
            int height = rows * maxHeight;
            string pathToCollage = util.MakePathToCollage(userId, albumId, rnd.Next(5, 15));
            string pathToCollages = util.BuildPathToUserAlbumCollagesFolderOnServer(userId, albumId);
            
            using (Image img = new Bitmap(width, height))
            {
                Graphics grfx = Graphics.FromImage(img);
                SetUpGraphics(grfx);

                TileTheImage(grfx, GetEnumerator(GetThumbnails()), width, height);

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


        public IEnumerable<string> GetEnumerator(IEnumerable<string> enumerable)
        {
            string[] arr = enumerable.ToArray();
            int length = arr.Length;

            List<int> indexes =
                Enumerable.Range(0, length).ToList();

            for (int iter = 0; iter < length; iter++)
            {
                int index = rnd.Next(0, length - iter);
                yield return arr[indexes[index]];
                indexes.RemoveAt(index);
            }
        }
    }
}