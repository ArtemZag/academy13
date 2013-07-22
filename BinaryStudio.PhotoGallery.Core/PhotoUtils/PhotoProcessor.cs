using System.Drawing;
using System.Drawing.Drawing2D;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public static class PhotoProcessor
    {
        public static Image ImageResize(Image image, int nHeight, int nWidth)
        {
            Image result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, nWidth, nHeight);
                g.Dispose();
            }
            return result;
        }

        public static Image ImageResize(Image image, int nHeight)
        {
            double coef = nHeight/(double) image.Height;
            var nWidth = (int) (image.Width*coef);
            Image result = ImageResize(image, nHeight, nWidth);
            return result;
        }
    }
}