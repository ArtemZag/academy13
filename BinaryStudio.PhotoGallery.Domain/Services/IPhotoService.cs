using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IPhotoService
    {
        /// <summary>
        ///     Adds photoModel by specidied user to his album.
        /// </summary>
        PhotoModel AddPhoto(string userEmail, string albumName, PhotoModel photoModel);

        PhotoModel AddPhoto(PhotoModel photoModel);

        /// <summary>
        ///     Adds photos by specidied user to his album.
        /// </summary>
        IEnumerable<int> AddPhotos(string userEmail, string albumName, IEnumerable<PhotoModel> photos);

        /// <summary>
        ///     Updates photo by photoModel
        /// </summary>
        PhotoModel UpdatePhoto(PhotoModel photoModel);

        /// <summary>
        ///     Deletes specified photoModel
        /// </summary>
        void DeletePhoto(int userId, PhotoModel photo);

        int PhotoCount(int userId);

        DateTime LastPhotoAdded(int userId);

        IEnumerable<PhotoModel> GetLastPhotos(int userId, int skipCount, int takeCount);
        /// <summary>
        ///     Returns specified interval of photos (sorted by date) from specified album.
        /// </summary>
        /// <param name="userEmail">Users email.</param>
        /// <param name="albumName">Album name.</param>
        /// <param name="skipCount">Beginning of the interval.</param>
        /// <param name="takeCount">Ending of the interval.</param>
        IEnumerable<PhotoModel> GetPhotos(string userEmail, string albumName, int skipCount, int takeCount);

        IEnumerable<PhotoModel> GetPhotos(int userId, int albumId, int skipCount, int takeCount);

        /// <summary>
        ///     Returns specified interval of users photos.
        /// </summary>
        /// <param name="userId">Users email.</param>
        /// <param name="skipCount">Beginning of the interval.</param>
        /// <param name="takeCount">Ending of the interval.</param>
        IEnumerable<PhotoModel> GetPhotos(int userId, int skipCount, int takeCount);

        PhotoModel GetPhoto(string userEmail, int photoId);

        PhotoModel GetPhoto(int userId, int photoId);


        /// <summary>
        ///     Gets all likes for photoModel
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        IEnumerable<UserModel> GetLikes(int userId, int photoId);


        /// <summary>
        ///     Add like to photoModel from user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="photoId"></param>
        void AddLike(int userId, int photoId);


        /// <summary>
        ///     Get all photos, visible by permissions to current user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skipCount">Offset of the first photoModel in list</param>
        /// <param name="takeCount">Number of photos to be returned</param>
        /// <returns>List of photos</returns>
        IEnumerable<PhotoModel> GetPublicPhotos(int userId, int skipCount, int takeCount);

        void DeletePhoto(int userId, int photoId);
    }
}