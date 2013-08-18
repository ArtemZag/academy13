using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Database;
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
        int GetAlbumId(string userEmail, string albumName);

        int GetAlbumId(int userId, string albumName);

        /// <summary>
        ///     Gets user's album specified by id.
        /// </summary>
        /// <param name="albumId">Album Id</param>
        AlbumModel GetAlbum(int albumId);

        /// <summary>
        ///     Gets all albums for specified user.
        /// </summary>
        IEnumerable<AlbumModel> GetAllAlbums(string userEmail);

        /// <summary>
        ///     Creates album for specified user by his email.
        /// </summary>
        AlbumModel CreateAlbum(int userId, AlbumModel album);

        AlbumModel CreateAlbum(string userEmail, string albumName);

        AlbumModel CreateAlbum(int userId, string albumName);

        /// <summary>
        ///     Deletes specified album.
        /// </summary>
        void DeleteAlbum(string userEmail, int albumId);

        /// <summary>
        ///     
        /// </summary>
        bool IsExist(string userEmail, string albumName);

        /// <summary>
        ///     Get all available albums for specified user
        /// </summary>
        IEnumerable<AlbumModel> GetAvailableAlbums(int userId);

        IEnumerable<AlbumModel> GetAvailableAlbums(string userEmail);

        IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork);
    }
}