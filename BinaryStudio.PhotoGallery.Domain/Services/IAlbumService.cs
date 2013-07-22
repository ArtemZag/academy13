using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IAlbumService
    {
        /// <summary>
        /// Gets users album specified by name. 
        /// </summary>
        AlbumModel GetAlbum(string userEmail, string albumName);

        /// <summary>
        /// Gets all albums for specified user. 
        /// </summary>
        ICollection<AlbumModel> GetAlbums(string userEmail); 

        /// <summary>
        /// Creates album for specified user by his email.
        /// </summary>
        void CreateAlbum(string userEmail, AlbumModel album);

        /// <summary>
        /// Updates info about album.
        /// </summary>
        /// <param name="album">Must to contain full info about album.</param>
        void UpdateAlbum(AlbumModel album);

        /// <summary>
        /// Deletes specified album.
        /// </summary>
        void DeleteAlbum(string userEmail, string albumName);
    }
}
