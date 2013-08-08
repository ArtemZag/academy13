namespace BinaryStudio.PhotoGallery.Core.IOUtils
{
    public interface IPathWrapper
    {
        string GetFullPath(string path);
        string GetFileName(string path);
        string GetFileNameWithoutExtension(string path);
    }
}