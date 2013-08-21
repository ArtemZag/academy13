using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoService : DbService, IPhotoService
    {
        private readonly ISecureService secureService;
        private readonly IGlobalEventsAggregator _eventsAggregator;

        public PhotoService(IUnitOfWorkFactory workFactory, ISecureService secureService, IGlobalEventsAggregator eventsAggregator) : base(workFactory)
        {
            this.secureService = secureService;
            _eventsAggregator = eventsAggregator;
        }

        public PhotoModel AddPhoto(PhotoModel photoModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(photoModel.OwnerId, photoModel.OwnerId, unitOfWork);

                album.Photos.Add(photoModel);

                unitOfWork.SaveChanges();

                return photoModel;
            }
        }

        public PhotoModel AddPhoto(string userEmail, string albumName, PhotoModel photoModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName);

                if (secureService.CanUserAddPhoto(user.Id, album.Id))
                {
                    photoModel.OwnerId = user.Id;
                    album.Photos.Add(photoModel);

                    unitOfWork.SaveChanges();

                    return photoModel;
                }

                throw new NoEnoughPrivilegesException("User can't get access to photos");
            }
        }

        public IEnumerable<int> AddPhotos(string userEmail, string albumName, IEnumerable<PhotoModel> photos)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName);

                if (secureService.CanUserAddPhoto(user.Id, album.Id))
                {
                    IEnumerable<PhotoModel> photoModels = photos as IList<PhotoModel> ?? photos.ToList();
                    foreach (PhotoModel photo in photoModels)
                    {
                        album.Photos.Add(photo);
                    }
                    unitOfWork.SaveChanges();

                    return photoModels.Select(photo => photo.Id).ToList();
                }

                throw new NoEnoughPrivilegesException("User can't get access to photos");
            }
        }

        public PhotoModel UpdatePhoto(PhotoModel photoModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Photos.Update(photoModel);

                unitOfWork.SaveChanges();

                return photoModel;
            }
        }

        public void DeletePhoto(string userEmail, PhotoModel photo)
        {
            DeletePhoto(userEmail, photo.Id);
        }

        public void DeletePhoto(string userEmail, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                if (secureService.CanUserDeletePhoto(user.Id, photoId))
                {
                    PhotoModel photoToDelete = unitOfWork.Photos.Find(model => model.Id == photoId);

                    photoToDelete.IsDeleted = true;

                    unitOfWork.SaveChanges();
                }
                else
                {
                    throw new NoEnoughPrivilegesException("User can't get access to photos");
                }
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, string albumName, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName);

                if (secureService.CanUserViewPhotos(user.Id, album.Id))
                {
                    return album.Photos.OrderBy(model => model.DateOfCreation)
                        .ThenBy(model => model.Id)
                        .Skip(skipCount)
                        .Take(takeCount)
                        .ToList();
                }

                throw new NoEnoughPrivilegesException("User can't get access to photos");
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, int albumId, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                if (secureService.CanUserViewPhotos(user.Id, albumId))
                {
                    return unitOfWork.Photos.Filter(model => model.AlbumId == albumId)
                        .Where(model => !model.IsDeleted)
                        .OrderByDescending(model => model.DateOfCreation)
                        .ThenBy(model => model.Id)
                        .Skip(skipCount)
                        .Take(takeCount)
                        .ToList();
                }

                throw new NoEnoughPrivilegesException("User can't get access to photos");
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                // todo: create a criterions for grabing all user albums' IDs and check every for access permissions

                return unitOfWork.Photos.Filter(model => model.OwnerId == user.Id)
                    .Where(model => !model.IsDeleted)
                    .OrderByDescending(model => model.DateOfCreation)
                    .ThenBy(model => model.Id)
                    .Skip(skipCount)
                    .Take(takeCount)
                    .ToList();
            }
        }

        public PhotoModel GetPhoto(string userEmail, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                PhotoModel photoModel = GetPhoto(user.Id, photoId);

                return photoModel;
            }
        }

        public PhotoModel GetPhoto(int userId, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                PhotoModel photoModel = unitOfWork.Photos.Find(photoId);

                if (secureService.CanUserViewPhotos(userId, photoModel.AlbumId))
                {
                    return photoModel;
                }

                throw new NoEnoughPrivilegesException("User can't get access to photos");
            }
        }

        public PhotoModel GetPhotoWithoutRightsCheck(int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                PhotoModel photoModel = unitOfWork.Photos.Find(photoId);
                return photoModel;
            }
        }


        public IEnumerable<UserModel> GetLikes(string userEmail, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                PhotoModel photo = unitOfWork.Photos.Find(photoId);

                if (secureService.CanUserViewLikes(user.Id, photo.AlbumId))
                {
                    return photo.Likes.ToList();
                }

                throw new NoEnoughPrivilegesException("User can't get access to photoModel's likes");
            }
        }

        public void AddLike(string userEmail, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                unitOfWork.Photos.Find(photoId).Likes.Add(user);
                unitOfWork.SaveChanges();

                _eventsAggregator.PushLikeToPhotoAddedEvent(user, photoId);
            }
        }

        public IEnumerable<PhotoModel> GetPublicPhotos(string userEmail, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                IEnumerable<AlbumModel> avialableAlbums = secureService.GetAvailableAlbums(user.Id, unitOfWork);

                return
                    avialableAlbums.SelectMany(model => model.Photos)
                        .OrderByDescending(model => model.DateOfCreation)
                        .ThenBy(photo => photo.Id)
                        .Skip(skipCount)
                        .Take(takeCount)
                        .ToList();
            }
        }
    }
}