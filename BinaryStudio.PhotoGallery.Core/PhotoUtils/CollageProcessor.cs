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

        public void CreateCollage(int userId, int albumId)
        {
            string collagesDirectoryPath = _pathUtil.BuildAbsoluteCollagesDirPath(userId, albumId);

            MakeCollage(userId, albumId, COLLAGE_WITH, COLLAGE_ROWS, collagesDirectoryPath);
        }

        private void MakeCollage(int userId, int albumId, int width, int rows, string collagesDirectoryPath)
        {
            int height = rows*MAX_HEIGHT;

            string collagePath = _pathUtil.BuildAbsoluteCollagePath(userId, albumId);

            using (Image image = new Bitmap(width, height))
            {
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    SetUpGraphics(graphics);


                    List<string> thumbnailsPaths = _pathUtil.BuildAbsoluteThumbnailsPaths(userId, albumId,
                                                                                          ImageSize.Small).ToList();

                    TileImages(graphics, thumbnailsPaths, width, height);

                    Directory.CreateDirectory(collagesDirectoryPath);
                    DeleteFile(collagePath);

                    image.Save(collagePath, ImageFormat.Jpeg);
                }
            }
        }
        private void DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {}
        }
        private void TileImages(Graphics graphics, IEnumerable<string> thumbnails, int width, int heigth)
        {
            int iter = 0;
            int sumWidth = 0;

            foreach (string file in thumbnails)
            {
                using (Image thumbImage = Image.FromFile(file))
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