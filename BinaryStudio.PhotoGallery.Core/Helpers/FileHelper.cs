using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BinaryStudio.PhotoGallery.Core.IOUtils;
using Winista.Mime;

namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IFileWrapper _fileWrapper;

        public FileHelper(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        public string GetMimeTypeOfFile(string fileName)
        {
            if (!_fileWrapper.Exists(fileName))
            {
                throw new FileNotFoundException(fileName + " not found");
            }

            var allMimeTypes = new MimeTypes();

            MimeType mimeType;

            try
            {
                mimeType = allMimeTypes.GetFileMimeType(fileName);
            }
            catch (Exception)
            {
                return null;
            }

            return mimeType != null ? mimeType.Name : "unknown/unknown";
        }

        public bool IsImageFile(string fileName)
        {
            var fileMime = GetMimeTypeOfFile(fileName);
            return fileMime.StartsWith("image/");
        }

        /// <summary>
        /// Equal two files by content using MD5 Hash
        /// </summary>
        /// <param name="firstFile">First path of the file to be compared</param>
        /// <param name="secondFile">Second path of the file to be compared</param>
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

        /// <summary>
        /// Rename file in path. If it's already exist, than to file will be added ' (i)',
        /// where i - number of file copies (as in a windows explorer)
        /// </summary>
        /// <param name="sourceName">path to source file</param>
        /// <param name="destName">path to destination file</param>
        public void HardMove(string sourceName, string destName)
        {
            if (sourceName == null)
            {
                throw new ArgumentNullException("sourceName");
            }

            if (destName == null)
            {
                throw new ArgumentNullException("destName");
            }

            if (!_fileWrapper.Exists(sourceName))
            {
                throw new FileNotFoundException(string.Format("Source file '{0}' not found", sourceName));
            }

            var destPath = Path.GetDirectoryName(destName);

            var fileName = new StringBuilder(Path.GetFileNameWithoutExtension(destName));
            var newFileNameWithPath = new StringBuilder();

            newFileNameWithPath.AppendFormat("{0}\\{1}{2}", destPath, fileName, Path.GetExtension(destName));

            long number = 1;

            // Create new file name, while it exist
            while (_fileWrapper.Exists(newFileNameWithPath.ToString()))
            {
                var leftBraketIndex = fileName.ToString().LastIndexOf('(');
                var rightBraketIndex = fileName.ToString().LastIndexOf(')');

                var numberLength = rightBraketIndex - leftBraketIndex - 1;

                if (leftBraketIndex == -1 || rightBraketIndex == -1 || numberLength <= 0)
                {
                    fileName.Append(" (1)");
                }
                else
                {
                    const string pattern = @"\s[(]\d+[)]$"; // ' (12)' oder ' (1)' and etc.

                    var regEx = new Regex(pattern);

                    var fileNumberFound = regEx.IsMatch(fileName.ToString());

                    if (fileNumberFound)
                    {
                        fileName.Remove(leftBraketIndex + 1, numberLength);
                        fileName.Insert(leftBraketIndex + 1, number);
                    }
                    else
                    {
                        fileName.Append(" (1)");
                    }
                }

                newFileNameWithPath.Clear();

                newFileNameWithPath.AppendFormat("{0}\\{1}{2}", destPath, fileName, Path.GetExtension(destName));

                number++;
            }

            _fileWrapper.Move(sourceName, newFileNameWithPath.ToString());
        }
    }
}
