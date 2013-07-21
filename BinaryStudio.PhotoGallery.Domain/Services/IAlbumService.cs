using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IAlbumService
    {
        // todo: too many questions about db
        //AlbumModel GetAlbum(UserModel model, string albumName);

        AlbumModel GetAlbum(string userEmail, string albumName);

        /// <summary>
        /// Creates album for specified user.
        /// </summary>
        void CreateAlbum(UserModel user, AlbumModel album);

        /// <summary>
        /// Creates album for specified user by his email.
        /// </summary>
        void CreateAlbum(string userEmail, AlbumModel album);

        /// <summary>
        /// Updates some albums info.
        /// </summary>
        void UpdateAlbum(AlbumModel album);

        /// <summary>
        /// Deletes specified album
        /// </summary>
        void DeleteAlbum(AlbumModel album);
    }
}
