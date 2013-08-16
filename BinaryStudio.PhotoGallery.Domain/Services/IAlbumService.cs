using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IAlbumService
    {
        /// <summary>
        ///     Gets user's album specified by name.
        /// </summary>
        AlbumModel GetAlbum(string userEmail, int albumId);

        /// <summary>
        ///     Gets album id specified by his name.
        /// </summary>
        int GetAlbumId(string albumName);

        /// <summary>
        ///     Gets user's album specified by id.
        /// </summary>
        /// <param name="albumId">Album ID</param>
        AlbumModel GetAlbum(int albumId);

        /// <summary>
        ///     Gets all albums for specified user.
        /// </summary>
        IEnumerable<AlbumModel> GetAllAlbums(string userEmail);

        /// <summary>
        ///     Creates album for specified user by his email.
        /// </summary>
        void CreateAlbum(string userEmail, AlbumModel album);

        void CreateAlbum(string userEmail, string albumName);

        /// <summary>
        ///     Deletes specified album.
        /// </summary>
        void DeleteAlbum(string userEmail, int albumId);

        bool IsExist(string albumName);
    }
}