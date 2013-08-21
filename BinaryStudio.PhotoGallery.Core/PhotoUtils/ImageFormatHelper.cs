using System.Collections.Generic;
using System.IO;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    static class ImageFormatHelper
    {
        private static readonly IEnumerable<string> photoFormats;
        static ImageFormatHelper()
        {
            photoFormats = new[] { "*.bmp", "*.ico", "*.gif", "*.jpeg", "*.jpg", "*.jfif", "*.png", "*.tif", "*.tiff", "*.wmf", "*.emf"};
        }
        public static IEnumerable<string> GetImages(string path)
        {
            var photos = new List<string>();
            foreach (var photoFormat in photoFormats)
            {
                photos.AddRange(Directory.GetFiles(path, photoFormat));
            }
            return photos;
        }
    }
}
