using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Database;
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

                photo.UserModelId = user.Id;
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
                unitOfWork.Photos.Find(photo).IsDeleted = true;

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
                         .ThenBy(model => model.Id)
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

        public string GetAlbumPath(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel album = unitOfWork.Albums.Find(model => model.Id == photo.AlbumModelId);
                UserModel user = unitOfWork.Users.Find(model => model.Id == album.UserModelId);

                return PathHelper.GetAlbumPath(user.Id, album.Id);
            }
        }
    }
}