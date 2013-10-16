using System.IO;

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

        public void Copy(string source, string destination)
        {
            File.Copy(source,destination);
        }
    }
}