using System.IO;

namespace BinaryStudio.PhotoGallery.Core.IOUtils
{
    public class PathWrapper : IPathWrapper
    {
        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}
