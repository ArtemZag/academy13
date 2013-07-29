using System;
using System.IO;
using Winista.Mime;

namespace BinaryStudio.PhotoGallery.Core.Helpers
{
   public static class FormatHelper
    {
       public static string GetMimeTypeOfFile(string fileName)
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

           var allMimeTypes = new MimeTypes("mime-types.xml");

           var mimeType = allMimeTypes.GetMimeType(fileData);

           return mimeType != null ? mimeType.Name : "unknown/unknown";
       }

       public static bool IsImageFile(string fileName)
       {
           var fileMime = GetMimeTypeOfFile(fileName);
           return fileMime.StartsWith("image/");
       }
    }
}
