using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Utils
{
    public interface IStorage
    {
        string GetAlbumPath(AlbumModel album);

        string GetAlbumPath(PhotoModel photo);

        string GetOriginalPhotoPath(PhotoModel photo);

        string GetThumbnailsPath(PhotoModel photo);
    }
}
