namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public interface IFileHelper
    {
        string GetMimeTypeOfFile(string fileName);
        bool IsImageFile(string fileName);
        void HardRename(string sourceName, string destName);
    }
}