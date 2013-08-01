using System;
using System.IO;
using Winista.Mime;

namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public class FormatHelper : IFormatHelper
    {
        public string GetMimeTypeOfFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName + " not found");
            }

            sbyte[] fileData;

            using (var srcFile = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var data = new byte[srcFile.Length];
                srcFile.Read(data, 0, (Int32) srcFile.Length);
                fileData = SupportUtil.ToSByteArray(data);
            }

            var allMimeTypes = new MimeTypes();

            var mimeType = allMimeTypes.GetMimeType(fileData);

            return mimeType != null ? mimeType.Name : "unknown/unknown";
        }

        public bool IsImageFile(string fileName)
        {
            var fileMime = GetMimeTypeOfFile(fileName);
            return fileMime.StartsWith("image/");
        }
    }
}