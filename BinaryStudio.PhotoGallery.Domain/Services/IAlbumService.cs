using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IAlbumService
    {
        /// <summary>
        ///     Gets user's album specified by name.
        /// </summary>
        AlbumModel GetAlbum(string userEmail, string albumName);
        
        /// <summary>
        /// Gets user's album specified by id.
        /// </summary>
        /// <param name="albumId">Album ID</param>
        /// <returns></returns>
        AlbumModel GetAlbumById(int albumId);
        
        /// <summary>
        ///     Gets all albums for specified user.
        /// </summary>
        IEnumerable<AlbumModel> GetAlbums(string userEmail);

        /// <summary>
        ///     Creates album for specified user by his email.
        /// </summary>
        void CreateAlbum(string userEmail, AlbumModel album);

        /// <summary>
        ///     Deletes specified album.
        /// </summary>
        void DeleteAlbum(string userEmail, string albumName);
    }
}