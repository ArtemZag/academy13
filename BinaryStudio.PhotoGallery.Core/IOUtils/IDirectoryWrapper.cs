using System.IO;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IDirectoryWrapper
    {
        bool Exists(string path);

        DirectoryInfo CreateDirectory(string path);
    }
}
