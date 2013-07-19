using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IAlbumService
    {
        void AddAlbum(AlbumModel albumModel);

        void DeleteAlbum(AlbumModel albumModel);
    }
}
