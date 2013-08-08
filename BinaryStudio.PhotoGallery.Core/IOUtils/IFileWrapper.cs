namespace BinaryStudio.PhotoGallery.Core.IOUtils
{
    public interface IFileWrapper
    {
        void Delete(string path);
        bool Exists(string path);
        void Move(string sourceFileName, string destFileName);
        bool Equals(string firstFile, string secondFile);
    }
}
