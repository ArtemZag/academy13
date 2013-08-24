using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    internal class CollageProcessor : ICollageProcessor
    {
        private void CreateDirectoriesIfNotExists(params string[] paths)
        {
            foreach (string path in paths.Where(path => !Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
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

        private void SetUpGraphics(Graphics grfx)
        {
            grfx.CompositingQuality = CompositingQuality.HighQuality;
            grfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grfx.SmoothingMode = SmoothingMode.HighQuality;
        }

        private string MakeCollage(int width, int rows)
        {
            int height = rows * maxHeight;
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
    }
}
