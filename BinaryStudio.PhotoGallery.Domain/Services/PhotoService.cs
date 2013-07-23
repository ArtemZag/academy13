using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Core.Helpers;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoService : Service, IPhotoService
    {
        public PhotoService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public void AddPhoto(string userEmail, string albumName, PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                photo.UserModelID = user.ID;
                album.Photos.Add(photo);

                unitOfWork.SaveChanges();
            }
        }

        public void AddPhotos(string userEmail, string albumName, ICollection<PhotoModel> photos)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                foreach (var photo in photos)
                {
                    album.Photos.Add(photo);
                }

                unitOfWork.SaveChanges();
            }
        }

        // todo: what about storage?
        public void DeletePhoto(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Photos.Delete(photo);

                unitOfWork.SaveChanges();
            }
        }

        public ICollection<PhotoModel> GetPhotos(string userEmail, string albumName, int begin, int end)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                return album.Photos.Skip(begin).Take(end - begin).ToList();
            }
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

            // for test only! todo: remove when real user photos will be added
            for (int i = 1; i < 20; i++)
                result.Add(new PhotoModel { PhotoThumbSource = PathHelper.ImageDir + "/test/" + i + ".jpg" });

            return result;
        }

        private AlbumModel GetAlbum(UserModel user, string albumName, IUnitOfWork unitOfWork)
        {
            AlbumModel result;

            try
            {
                result =
                    unitOfWork.Albums.Find(
                        model => model.UserModelID == user.ID && string.Equals(model.AlbumName, albumName));
            }
            catch (Exception e)
            {
                throw new AlbumNotFoundException(e);
            }

            return result;
        }
    }
}