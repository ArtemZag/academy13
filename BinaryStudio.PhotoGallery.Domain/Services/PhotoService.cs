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

        public bool AddPhoto(string userEmail, PhotoModel photo)
        {
            throw new System.NotImplementedException();
        }

        public bool AddPhotos(string userEmail, ICollection<PhotoModel> photos)
        {
            throw new System.NotImplementedException();
        }

        public bool DeletePhoto(int photoId)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<PhotoModel> GetPhotos(string userEmail, int @from, int to)
        {
            throw new System.NotImplementedException();
        }
    }
}