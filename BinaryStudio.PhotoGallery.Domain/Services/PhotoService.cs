using System.Collections.Generic;
using System.Linq;
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

        public void DeletePhoto(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                PhotoModel photoToDelete = unitOfWork.Photos.Find(model => model.Id == photo.Id);
                photoToDelete.IsDeleted = true;

                unitOfWork.SaveChanges();
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, string albumName, int begin, int end)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                return album.Photos.OrderBy(model => model.DateOfCreation)
                            .ThenBy(model => model.Id)
                            .Skip(begin)
                            .Take(end - begin);
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, int begin, int end)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                IQueryable<PhotoModel> result = unitOfWork.Photos.Filter(model => model.UserModelId == user.Id)
                                                          .Where(model => !model.IsDeleted)
                                                          .OrderBy(model => model.DateOfCreation)
                                                          .ThenBy(model => model.Id)
                                                          .Skip(begin)
                                                          .Take(end - begin);

                return result.ToList();
            }
        }
    }
}