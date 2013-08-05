using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BinaryStudio.PhotoGallery.Core.IOUtils
{
    public class FileWrapper : IFileWrapper
    {
        public void Delete(string path)
        {
            File.Delete(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        /// <summary>
        /// Equal two files using MD5 Hash
        /// </summary>
        /// <param name="firstFile">First file to be compared</param>
        /// <param name="secondFile">Second file to be compared</param>
        /// <returns>true if content of the files are equal</returns>
        public bool Equals(string firstFile, string secondFile)
        {
            byte[] firstFileHash;
            byte[] secondFileHash;

            using (var md5 = MD5.Create())
            {
                using (var sourceStream = File.OpenRead(firstFile))
                using (var destStream = File.OpenRead(secondFile))
                {
                    firstFileHash = md5.ComputeHash(sourceStream);
                    secondFileHash = md5.ComputeHash(destStream);
                }
            }

            return !firstFileHash.Where((t, index) => t != secondFileHash[index]).Any();
        }

        
    }
}