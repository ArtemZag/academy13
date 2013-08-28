using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Core.PathUtils;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public class PhotoProcessor : IPhotoProcessor
    {
        private readonly IPathUtil _pathUtil;
        private readonly ParallelOptions options;
        public PhotoProcessor(IPathUtil pathUtil)
        {
            _pathUtil = pathUtil;
            options = new ParallelOptions() {MaxDegreeOfParallelism = Environment.ProcessorCount};
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
                /* Parallel.Invoke(options,
                    () => CreateThumbnail(userId, albumId, photoId, format, ImageSize.Big), 
                    () => CreateThumbnail(userId, albumId, photoId, format, ImageSize.Medium), 
                    () => CreateThumbnail(userId, albumId, photoId, format, ImageSize.Small));*/
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
                Parallel.Invoke(options,
                    () => CreateAvatarThumbnail(userId, originalAvatarPath, ImageSize.Big),
                    () => CreateAvatarThumbnail(userId, originalAvatarPath, ImageSize.Medium),
                    () => CreateAvatarThumbnail(userId, originalAvatarPath, ImageSize.Small));
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
                Size size = CalculateThumbnailSize(image.Size, maxSize);
                using (Image newImage = new Bitmap(size.Width, size.Height))
                {
                    using (Graphics grfx = Graphics.FromImage(newImage))
                    {
                        grfx.CompositingQuality = CompositingQuality.HighQuality;
                        grfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        grfx.SmoothingMode = SmoothingMode.AntiAlias;
                        grfx.DrawImage(image, 0, 0, size.Width, size.Height);
                    }
                    newImage.Save(thumbnailPath);
                }
            }
        }

        private Size CalculateThumbnailSize(Size size, int maxSize)
        {
            var height = size.Height > maxSize ? maxSize : size.Height;
            var koef = (double) height/size.Height;
            var width = (int)(size.Width*koef);

            return new Size(width, height);
        }
    }
}