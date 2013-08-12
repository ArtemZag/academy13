using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IPhotoService
    {
        /// <summary>
        ///     Adds photo by specidied user to his album.
        /// </summary>
        void AddPhoto(string userEmail, string albumName, PhotoModel photo);

        /// <summary>
        ///     Adds photos by specidied user to his album.
        /// </summary>
        void AddPhotos(string userEmail, string albumName, IEnumerable<PhotoModel> photos);

        /// <summary>
        ///     Deletes specified photo
        /// </summary>
        void DeletePhoto(string userEmail, PhotoModel photo);

        /// <summary>
        ///     Returns specified interval of photos (sorted by date) from specified album.
        /// </summary>
        /// <param name="userEmail">Users email.</param>
        /// <param name="albumName">Album name.</param>
        /// <param name="begin">Beginning of the interval.</param>
        /// <param name="end">Ending of the interval.</param>
        IEnumerable<PhotoModel> GetPhotos(string userEmail, string albumName, int begin, int end);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="albumID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        IEnumerable<PhotoModel> GetPhotos(string userEmail, int albumID, int begin, int end);

        /// <summary>
        ///     Returns specified interval of users photos.
        /// </summary>
        /// <param name="userEmail">Users email.</param>
        /// <param name="begin">Beginning of the interval.</param>
        /// <param name="end">Ending of the interval.</param>
        IEnumerable<PhotoModel> GetPhotos(string userEmail, int begin, int end);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        PhotoModel GetPhoto(string userEmail, int photoID);
    }
}