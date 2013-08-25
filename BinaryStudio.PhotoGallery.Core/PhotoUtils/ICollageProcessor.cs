namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public interface ICollageProcessor
    {
        void CreateCollageIfNotExist(int userId, int albumId, int width, int rows);
    }
}