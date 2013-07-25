namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    internal interface IAsyncPhotoProcessor
    {
        string CreateThumbnailsIfNotExist();
        string DeleteThumbnails();
        string DeleteThumbnailForSpecifiedOriginalFile(string name);
        string DeleteThumbnailsIfOriginalNotExist();
        string SyncOriginalAndThumbnailImages();
        string[] GetRandomThumbnail(int howMany);
        string[] GetThumbnails(string searchPattern = "*.*");
        string MakePrewiew(int width, int rows);
        string[] GetPrewiews();
        void SetUpForRandomEnumerable(string[] arr);
    }
}