namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public interface ICollageProcessor
    {
        string CreateCollageIfNotExist(int width, int rows);
    }
}