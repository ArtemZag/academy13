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

        PhotoModel AddPhoto(PhotoModel photoModel);

        /// <summary>
        ///     Adds photos by specidied user to his album.
        /// </summary>
        IEnumerable<int> AddPhotos(int userId, int albumId, IEnumerable<PhotoModel> photos);

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
        IEnumerable<PhotoModel> GetPhotos(int userId, int albumId, int skipCount, int takeCount);

        /// <summary>
        ///     Returns specified interval of users photos.
        /// </summary>
        /// <param name="userId">Users email.</param>
        /// <param name="skipCount">Beginning of the interval.</param>
        /// <param name="takeCount">Ending of the interval.</param>
        IEnumerable<PhotoModel> GetPhotos(int userId, int skipCount, int takeCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="photoId"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        IEnumerable<PhotoModel> GetPhotosByTags(int userId, int photoId, int skipCount, int takeCount);

        PhotoModel GetPhoto(int userId, int photoId);

        // For server side use, not for user access
        PhotoModel GetPhotoWithoutRightsCheck(int photoId);


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