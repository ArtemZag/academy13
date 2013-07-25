using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IPhotoService
    {
        /// <summary>
        /// Adds photo by specidied user to his album.  
        /// </summary>
        void AddPhoto(string userEmail, string albumName, PhotoModel photo);

        /// <summary>
        /// Adds photos by specidied user to his album.  
        /// </summary>
        void AddPhotos(string userEmail, string albumName, IEnumerable<PhotoModel> photos);

        /// <summary>
        /// Deletes 
        /// </summary>
        void DeletePhoto(PhotoModel photo);

        /// <summary>
        /// Returns specified interval of photos (sorted by date).
        /// </summary>
        /// <param name="userEmail">Users email.</param>
        /// <param name="albumName">Album name.</param>
        /// <param name="begin">Beginning of the interval.</param>
        /// <param name="end">Ending of the interval.</param>
        IEnumerable<PhotoModel> GetPhotos(string userEmail, string albumName, int begin, int end);

        /// <summary>
        /// Returns last N users photos.
        /// </summary>
        /// <param name="userEmail">Users email.</param>
        /// <param name="count">Number of photos.</param>
        List<PhotoModel> GetPhotos(string userEmail, int count);
    }
}
