namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    internal interface IAsyncPhotoProcessor
    {
        void CreateThumbnailsIfNotExist();
        void DeleteThumbnails();
        void DeleteThumbnailForSpecifiedOriginalFile(string name);
        void DeleteThumbnailsIfOriginalNotExist();
        void SyncOriginalAndThumbnailImages();
        string[] GetRandomThumbnail(int howMany);
        string[] GetThumbnails(string searchPattern = "*.*");
        string MakePrewiew(int width, int rows);
        string[] GetPrewiews();
        void SetUpForRandomEnumerable(string[] arr);
    }
}