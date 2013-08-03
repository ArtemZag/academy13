namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    internal interface IAsyncPhotoProcessor
    {
        void CreateThumbnailsIfNotExist();
        void DeleteThumbnails();
        string[] GetTextures();
        void DeleteThumbnailForSpecifiedOriginalFile(string name);
        void DeleteThumbnailsIfOriginalNotExist();
        void SyncOriginalAndThumbnailImages();
        string[] GetRandomThumbnail(int howMany);
        string[] GetThumbnails(string searchPattern = "*.*");
        string MakeCollage(int width, int rows, int margin);
        string[] GetCollages();
        void SetUpForRandomEnumerable(string[] arr);
    }
}