using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal interface ISecureService
    {
        /// <summary>
        ///     Checks if user have enough permissions to view comments in the album
        /// </summary>
        bool CanUserViewComments(int userId, int albumId);

        /// <summary>
        ///     Checks if user have enough permissions to add comment in the album
        /// </summary>
        bool CanUserAddComment(int userId, int albumId);

        /// <summary>
        ///     Checks if user have enough permissions to view photos in the album
        /// </summary>
        bool CanUserViewPhotos(int userId, int albumId);

        /// <summary>
        ///     Checks if user have enough permissions to add photo in the album
        /// </summary>
        bool CanUserAddPhoto(int userId, int albumId);

        /// <summary>
        ///     Checks if user have enough permissions to delete photo
        /// </summary>
        bool CanUserDeletePhoto(int userId, int photoId);

        /// <summary>
        ///     Gets a list of all available albums for user
        /// </summary>
        IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork);
    }
}