using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    internal class CollageProcessor : ICollageProcessor
    {
        private const int MAX_HEIGHT = 64;

        private const int COLLAGE_WITH = 256;

        private const int COLLAGE_ROWS = 3;

        private readonly IPathUtil _pathUtil;

        public CollageProcessor(IPathUtil pathUtil)
        {
            _pathUtil = pathUtil;
        }

        public void CreateCollage(int userId, int albumId, IEnumerable<PhotoModel> models)
        {
            string collagesDirectoryPath = _pathUtil.BuildAbsoluteCollagesDirPath(userId, albumId);

            MakeCollage(userId, albumId, COLLAGE_WITH, COLLAGE_ROWS, collagesDirectoryPath,models);
        }
        
        private void MakeCollage(int userId, int albumId, int width, int rows, string collagesDirectoryPath, IEnumerable<PhotoModel> models)
        {
            int height = rows*MAX_HEIGHT;
            
            string collagePath = _pathUtil.CreateCollagePath(userId, albumId);

            using (Image image = new Bitmap(width, height))
            {
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    SetUpGraphics(graphics);

                    IEnumerable<string> thumbnailsPaths = _pathUtil.BuildAbsoluteThumbnailsPaths(userId, albumId, models,
                                                                                                 ImageSize.Small);

                    var backup = _pathUtil.BuildAbsoluteThumbnailsPaths(userId, albumId, models, ImageSize.Big).First();

                    TileImages(graphics, thumbnailsPaths, backup, width, height);

                    if (Directory.Exists(collagesDirectoryPath))
                        ClearDirectory(collagesDirectoryPath);
                    else
                        Directory.CreateDirectory(collagesDirectoryPath);
                    
                    image.Save(collagePath, ImageFormat.Jpeg);
                }
            }
        }
        private void ClearDirectory(string path)
        {
            foreach (var variable in Directory.EnumerateFiles(path))
            {
                try
                {
                    File.Delete(variable);
                }
                catch
                {
                }
            }
        }
        private void TileImages(Graphics graphics, IEnumerable<string> thumbnails, string biggestFile, int width,
            int heigth)
        {
            var countPhotos = thumbnails.Count();
            var iter = 0;
            var sumWidth = 0;

            if (countPhotos <= 12)
            {
                using (Image thumbImage = Image.FromFile(biggestFile))
                {
                    graphics.DrawImage(thumbImage, 0, 0, graphics.VisibleClipBounds.Width,
                        graphics.VisibleClipBounds.Height);
                }
            }
            else
            {
                foreach (var file in thumbnails)
                {
                    using (var thumbImage = Image.FromFile(file))
                    {
                        graphics.DrawImageUnscaled(thumbImage, sumWidth, iter);
                        sumWidth += thumbImage.Width;
                        if (sumWidth >= width)
                        {
                            sumWidth = 0;
                            iter += MAX_HEIGHT;
                            if (iter >= heigth)
                                break;
                        }
                    }
                }
            }
        }

        private void SetUpGraphics(Graphics graphics)
        {
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            graphics.FillRectangle(new SolidBrush(Color.WhiteSmoke), 0, 0, graphics.VisibleClipBounds.Width,
                graphics.VisibleClipBounds.Height);
        }
    }
}