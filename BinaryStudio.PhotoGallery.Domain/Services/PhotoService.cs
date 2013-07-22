using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
        }

        public bool AddPhotos(string userEmail, string albumName, ICollection<PhotoModel> photos)
        {
            throw new NotImplementedException();
        }

        public bool DeletePhoto(int photoId)
        {
            throw new NotImplementedException();
        }

        public ICollection<PhotoModel> GetPhotos(string userEmail, string albumName, int begin, int end)
        {
            throw new NotImplementedException();
        }

        public ICollection<PhotoModel> GetPhotos(string userEmail, int count)
        {
            var result = new List<PhotoModel>();

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                List<PhotoModel> photos = unitOfWork.Photos.Filter(model => model.UserModelID == user.ID).ToList();

                if (photos.Count < count)
                {
                    count = photos.Count;
                }

                var random = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < count; i++)
                {
                    int randomIndex = random.Next(photos.Count);

                    result.Add(photos[randomIndex]);
                }
            }

            return result;
        }
    }
}