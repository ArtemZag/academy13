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
        ///     Checks if user have enough permissions to view likes in the album
        /// </summary>
        bool CanUserViewLikes(int userId, int albumId);

        /// <summary>
        ///     Gets a list of all available albums for user
        /// </summary>
        IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork);


        /// <summary>
        ///     Lets group of users view comments in the album
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="albumId"></param>
        /// <param name="let"></param>
        void LetGroupViewComments(int userId, int groupId, int albumId, bool let);

        /// <summary>
        ///     Lets group of users add comment in the album
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="albumId"></param>
        /// <param name="let"></param>
        /// <returns></returns>
        bool LetGroupAddComment(int userId, int groupId, int albumId, bool let);

        /// <summary>
        ///     Lets group of users view photos in the album
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="albumId"></param>
        /// <param name="let"></param>
        /// <returns></returns>
        bool LetGroupViewPhotos(int userId, int groupId, int albumId, bool let);

        /// <summary>
        ///     Lets group of users add photo in the album
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="albumId"></param>
        /// <param name="let"></param>
        /// <returns></returns>
        bool LetGroupAddPhoto(int userId, int groupId, int albumId, bool let);

        /// <summary>
        ///     Lets group of users view likes in the album
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="albumId"></param>
        /// <param name="let"></param>
        /// <returns></returns>
        bool LetGroupViewLikes(int userId, int groupId, int albumId, bool let);
    }
}