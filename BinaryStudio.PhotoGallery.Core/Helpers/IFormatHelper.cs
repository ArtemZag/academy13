namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public interface IFormatHelper
    {
        string GetMimeTypeOfFile(string fileName);
        bool IsImageFile(string fileName);
    }
}
