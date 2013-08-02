using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Rename file in path. If it's already exist, than to file will be added ' (i)',
        /// where i - number of file copies (as in a windows explorer)
        /// </summary>
        /// <param name="sourceName">path to source file</param>
        /// <param name="destName">path to destination file</param>
        public void HardRename(string sourceName, string destName)
        {
            if (!File.Exists(sourceName))
            {
                throw new FileNotFoundException(string.Format("Source file '{0}' not found", sourceName));
            }

            if (destName == null)
            {
                throw new ArgumentNullException("destName");
            }

            var destParts = destName.Split('\\');
            var sourceParts = sourceName.Split('\\');

            if (destParts.Where((t, index) => t != sourceParts[index]).Any() || destParts.Length != sourceParts.Length)
            {
                throw new FileRenameException("Can't rename file in different folders");
            }

            var fileName = new StringBuilder(destParts.LastOrDefault());

            var newFileName = new StringBuilder(destName);

            long number = 1;

            // Create new file name, while it exist
            while (File.Exists(newFileName.ToString()))
            {
                if (this.Equals(sourceName, newFileName.ToString()))
                {
                    throw new FileRenameException("File already exist");
                }

                var leftBraketIndex = fileName.ToString().LastIndexOf('(');
                var rightBraketIndex = fileName.ToString().LastIndexOf(')');

                var point = fileName.ToString().LastIndexOf('.');

                var numberLength = rightBraketIndex - leftBraketIndex - 1;

                if (leftBraketIndex == -1 || rightBraketIndex == -1 || numberLength <= 0)
                {
                    if (point == -1)
                    {
                        fileName.Append(" (1)");
                    }
                    else
                    {
                        fileName.Insert(point, " (1)");
                    }
                }
                else
                {
                    var numberAsChars = new char[numberLength];

                    fileName.CopyTo(leftBraketIndex + 1, numberAsChars, 0, numberLength);

                    var numberAsString = new string(numberAsChars);

                    try
                    {
                        long foundNumber = Convert.ToUInt32(numberAsString);

                        if (point == -1)
                        {
                            fileName.AppendFormat(" ({0})", number);
                        }
                        else
                        {
                            fileName.Remove(leftBraketIndex+1, numberLength);
                            fileName.Insert(leftBraketIndex+1, number);
                        }
                    }
                    catch
                    {
                        if (point == -1)
                        {
                            fileName.Append(" (1)");
                        }
                        else
                        {
                            fileName.Insert(point, " (1)");
                        }
                    }
                }

                destParts[destParts.Length - 1] = fileName.ToString();

                newFileName.Clear();

                for (int index = 0; index < destParts.Length; index++)
                {
                    newFileName.AppendFormat(index + 1 != destParts.Length ? "{0}\\" : "{0}", destParts[index]);
                }

                number++;
            }

            File.Move(sourceName, newFileName.ToString());
        }
    }
}