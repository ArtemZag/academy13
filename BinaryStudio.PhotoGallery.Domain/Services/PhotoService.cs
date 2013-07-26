using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoService : DbService, IPhotoService
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

        public void AddPhotos(string userEmail, string albumName, IEnumerable<PhotoModel> photos)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                foreach (PhotoModel photo in photos)
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

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, string albumName, int begin, int end)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                return
                    album.Photos.OrderBy(model => model.DateOfCreation)
                         .ThenBy(model => model.ID)
                         .Skip(begin)
                         .Take(end - begin);
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, int begin, int end)
        {
            // real code block 
            /*
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                return 
                    unitOfWork.Photos.Filter(model => model.UserModelID == user.ID)
                              .OrderBy(model => model.DateOfCreation)
                              .ThenBy(model => model.ID)
                              .Skip(begin).Take(end - begin);
            }
            */

            // for test only!
            // todo: remove when real user photos will be added
            var test = new List<PhotoModel>();
            for (int i = 1; i < 20; i++)
                test.Add(new PhotoModel {PhotoThumbSource = PathHelper.ImageDir + "/test/" + i + ".jpg"});

            return test;
        }

        private AlbumModel GetAlbum(UserModel user, string albumName, IUnitOfWork unitOfWork)
        {
            try
            {
                return
                    unitOfWork.Albums.Find(
                        model => model.UserModelID == user.ID && string.Equals(model.AlbumName, albumName));
            }
            catch (Exception e)
            {
                throw new AlbumNotFoundException(e);
            }
        }
    }
}