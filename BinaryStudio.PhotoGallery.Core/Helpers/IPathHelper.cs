namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public interface IPathHelper
    {
        string DataDirectory { get; }

        string BuildPhotoDirectoryPath();

        string BuildAlbumPath(int userId, int albumId);

        string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string photoFormat);

        string BuildThumbnailsPath(int userId, int albumId);

        string BuildTemporaryDirectoryPath(string userDirectoryPath);
    }
}
