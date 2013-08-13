using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    interface ISecureService
    {
        /// <summary>
        /// Checks if user have enough permissions to view comments in the album
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="albumId"></param>
        /// <returns></returns>
        bool CanUserViewComments(int userId, int albumId);

        /// <summary>
        /// Checks if user have enough permissions to add comment in the album
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="albumId"></param>
        /// <returns></returns>
        bool CanUserAddComment(int userId, int albumId);

        /// <summary>
        /// Checks if user have enough permissions to view photos in the album
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="albumId"></param>
        /// <returns></returns>
        bool CanUserViewPhotos(int userId, int albumId);

        /// <summary>
        /// Checks if user have enough permissions to add photo in the album
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="albumId"></param>
        /// <returns></returns>
        bool CanUserAddPhoto(int userId, int albumId);

        /// <summary>
        /// Checks if user have enough permissions to delete photo
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        bool CanUserDeletePhoto(int userId, int photoId);

        /// <summary>
        /// Gets a list of all available albums for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<AlbumModel> GetAvailableAlbumsForUser(int userId);
    }
}
