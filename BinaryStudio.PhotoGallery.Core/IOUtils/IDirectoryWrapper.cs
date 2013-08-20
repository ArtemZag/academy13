using System.IO;

namespace BinaryStudio.PhotoGallery.Core.IOUtils
{
    public interface IDirectoryWrapper
    {
        bool Exists(string path);

        DirectoryInfo CreateDirectory(string path);
    }
}
