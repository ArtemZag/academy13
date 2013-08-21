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

        int AlbumsCount(int userId);

        IEnumerable<AlbumModel> GetAlbumsRange(int userId, int skipCount, int takeCount);

        IEnumerable<AlbumTagModel> GetTags(int albumId);

        /// <summary>
        ///     Gets all albums for specified user.
        /// </summary>
        IEnumerable<AlbumModel> GetAllAlbums(int userId);

        /// <summary>
        ///     Creates album for specified user by his email.
        /// </summary>
        AlbumModel CreateAlbum(int userId, AlbumModel album);

        AlbumModel CreateAlbum(string userEmail, string albumName);

        AlbumModel CreateAlbum(int userId, string albumName);

        void CreateSystemAlbums(int userId);

        /// <summary>
        ///     Deletes specified album.
        /// </summary>
        void DeleteAlbum(string userEmail, int albumId);

        /// <summary>
        ///     
        /// </summary>
        bool IsExist(int userId, string albumName);

        /// <summary>
        ///     Get all available albums for specified user
        /// </summary>
        IEnumerable<AlbumModel> GetAvailableAlbums(int userId);

        IEnumerable<AlbumModel> GetAvailableAlbums(string userEmail);

        IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork);
    }
}