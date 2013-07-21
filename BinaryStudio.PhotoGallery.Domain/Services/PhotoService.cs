using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoService : Service, IPhotoService
    {
        public PhotoService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public bool AddPhoto(string userEmail, string albumName, PhotoModel photo)
        {
            throw new System.NotImplementedException();
        }

        public bool AddPhotos(string userEmail, string albumName, ICollection<PhotoModel> photos)
        {
            throw new System.NotImplementedException();
        }

        public bool DeletePhoto(int photoId)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<PhotoModel> GetPhotos(string userEmail, string albumName, int begin, int end)
        {
            throw new System.NotImplementedException();
        }
    }
}