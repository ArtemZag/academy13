namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public interface IFileHelper
    {
        string GetMimeTypeOfFile(string fileName);
        bool IsImageFile(string fileName);
        bool Equals(string firstFile, string secondFile);
        void HardRename(string sourceName, string destName);
    }
}