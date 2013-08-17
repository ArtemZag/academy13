using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IPhotoService
    {
        /// <summary>
        ///     Adds photo by specidied user to his album.
        /// </summary>
        PhotoModel AddPhoto(string userEmail, string albumName, PhotoModel photo);

        PhotoModel AddPhoto(PhotoModel photo);

        /// <summary>
        ///     Adds photos by specidied user to his album.
        /// </summary>
        IEnumerable<int> AddPhotos(string userEmail, string albumName, IEnumerable<PhotoModel> photos);
        
        /// <summary>
        ///     Deletes specified photo
        /// </summary>
        void DeletePhoto(string userEmail, PhotoModel photo);

        /// <summary>
        ///     Returns specified interval of photos (sorted by date) from specified album.
        /// </summary>
        /// <param name="userEmail">Users email.</param>
        /// <param name="albumName">Album name.</param>
        /// <param name="skipCount">Beginning of the interval.</param>
        /// <param name="takeCount">Ending of the interval.</param>
        IEnumerable<PhotoModel> GetPhotos(string userEmail, string albumName, int skipCount, int takeCount);

        IEnumerable<PhotoModel> GetPhotos(string userEmail, int albumId, int skipCount, int takeCount);

        /// <summary>
        ///     Returns specified interval of users photos.
        /// </summary>
        /// <param name="userEmail">Users email.</param>
        /// <param name="skipCount">Beginning of the interval.</param>
        /// <param name="takeCount">Ending of the interval.</param>
        IEnumerable<PhotoModel> GetPhotos(string userEmail, int skipCount, int takeCount);

        PhotoModel GetPhoto(string userEmail, int photoID);


        /// <summary>
        /// Gets all likes for photo
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        IEnumerable<UserModel> GetLikes(string userEmail, int photoID);


        /// <summary>
        /// Add like to photo from user
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="photoID"></param>
        void AddLike(string userEmail, int photoID);


        /// <summary>
        /// Get all photos, visible by permissions to current user
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="skipCount">Offset of the first photo in list</param>
        /// <param name="takeCount">Number of photos to be returned</param>
        /// <returns>List of photos</returns>
        IEnumerable<PhotoModel> GetPublicPhotos(string userEmail, int skipCount, int takeCount);
    }
}