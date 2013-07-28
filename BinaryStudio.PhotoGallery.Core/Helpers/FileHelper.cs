using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Core.Helpers
{
   public static class FileHelper
    {
       public static string GetMimeTypeOfFile(string fileName)
       {
           /*if (!File.Exists(fileName))
           {
               throw new FileNotFoundException(fileName + " not found");
           }*/

           return "image/png";
       }

       public static bool IsImageFile(string fileName)
       {
           var fileMime = GetMimeTypeOfFile(fileName);
           return fileMime.StartsWith("image/");
       }
    }
}
