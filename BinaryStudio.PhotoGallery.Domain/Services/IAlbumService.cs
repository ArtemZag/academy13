using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IAlbumService
    {
        bool CreateAlbum(AlbumModel album);

        bool UpdateAlbum(AlbumModel album);

        bool DeleteAlbum(AlbumModel album);
    }
}
