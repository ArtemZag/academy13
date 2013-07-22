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
            List<PhotoModel> result;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                result =
                    unitOfWork.Photos.Filter(model => model.UserModelID == user.ID).Take(count).ToList();
            }

            return result;
        }
    }
}