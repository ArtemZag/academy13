using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BinaryStudio.PhotoGallery.Core
{
    public static class PhotoProcessor
    {
        public static Image ImageResize(Image image, int nHeight, int nWidth)
        {
            Image result = new Bitmap(nWidth, nHeight);
            using (var g = Graphics.FromImage(result))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, nWidth, nHeight);
            }
            return result;
        }

        public static Image ImageResize(Image image, int nHeight )
        {
            var coef = (double)nHeight/(double)image.Height;
            var nWidth = (int)(image.Width*coef);
            Image result = ImageResize(image, nHeight, nWidth);
            return result;  
        }

    }
}
