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
        /// <param name="userID"></param>
        /// <param name="albumID"></param>
        /// <returns></returns>
        bool CanUserViewComments(int userID, int albumID);

        /// <summary>
        /// Checks if user have enough permissions to add comment in the album
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="albumID"></param>
        /// <returns></returns>
        bool CanUserAddComment(int userID, int albumID);

        /// <summary>
        /// Checks if user have enough permissions to view photos in the album
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="albumID"></param>
        /// <returns></returns>
        bool CanUserViewPhotos(int userID, int albumID);

        /// <summary>
        /// Checks if user have enough permissions to add photo in the album
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="albumID"></param>
        /// <returns></returns>
        bool CanUserAddPhoto(int userID, int albumID);

        /// <summary>
        /// Checks if user have enough permissions to delete photo
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        bool CanUserDeletePhoto(int userID, int photoID);

        /// <summary>
        /// Gets a list of all available albums for user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IEnumerable<AlbumModel> GetAvailableAlbumsForUser(int userID);
    }
}
