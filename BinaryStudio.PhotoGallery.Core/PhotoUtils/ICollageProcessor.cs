namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public interface ICollageProcessor
    {
        void CreateCollage(int userId, int albumId, int width, int rows);
    }
}