using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IPhotoService
    {
        bool AddPhoto(string userEmail, string albumName, PhotoModel photo);

        bool AddPhotos(string userEmail, string albumName, ICollection<PhotoModel> photos);

        bool DeletePhoto(int photoId);

        /// <summary>
        /// Returns specified interval of photos (sorted by date).
        /// </summary>
        /// <param name="userEmail">Users email.</param>
        /// <param name="albumName">Album name.</param>
        /// <param name="from">Beginning of the interval.</param>
        /// <param name="to">Ending of the interval.</param>
        ICollection<PhotoModel> GetPhotos(string userEmail, string albumName, int from, int to);
    }
}
