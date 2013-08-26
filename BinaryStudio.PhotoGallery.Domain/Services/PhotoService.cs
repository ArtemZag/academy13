using System;
using System.Collections.Concurrent;
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
        private readonly ISecureService _secureService;
        private readonly IGlobalEventsAggregator _eventsAggregator;
        private readonly IMaskHelper _maskHelper;

        private Dictionary<int, PhotoModel> _publicPhotos;

        public PhotoService(IUnitOfWorkFactory workFactory, ISecureService secureService, IGlobalEventsAggregator eventsAggregator, IMaskHelper maskHelper) : base(workFactory)
        {
            _secureService = secureService;
            _eventsAggregator = eventsAggregator;
            _maskHelper = maskHelper;
        }

        private Dictionary<int, PhotoModel> PublicPhotos 
        { 
            get
            {
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    return _publicPhotos ?? (_publicPhotos =
                        unitOfWork.Albums.Filter(x => ((int)AlbumModel.PermissionsMask.PublicAlbum & x.Permissions)== x.Permissions)
                                         .SelectMany(model => model.Photos)
                                         .ToDictionary(mPhoto => mPhoto.Id, mPhoto => mPhoto));
                }
            } 
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

        public IEnumerable<int> AddPhotos(int userId, int albumId, IEnumerable<PhotoModel> photos)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(userId, albumId);

                var notAllowed = !_secureService.CanUserAddPhoto(userId, album.Id);

                if (notAllowed)
                {
                    throw new NoEnoughPrivilegesException("User can't get access to photos");
                }

                IEnumerable<PhotoModel> photoModels = photos as IList<PhotoModel> ?? photos.ToList();
                foreach (PhotoModel photo in photoModels)
                {
                    album.Photos.Add(photo);
                }
                unitOfWork.SaveChanges();

                return photoModels.Select(photo => photo.Id).ToList();
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

        public void DeletePhoto(int userId, PhotoModel photo)
        {
            DeletePhoto(userId, photo.Id);
        }

        public void DeletePhoto(int userId, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                if (_secureService.CanUserDeletePhoto(userId, photoId))
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

        public int PhotoCount(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                return unitOfWork.Photos.Filter(model => model.OwnerId == userId && !model.IsDeleted).Count();
        }

        public IEnumerable<PhotoModel> GetLastPhotos(int userId, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Photos.Filter(model => model.OwnerId == userId)
                    .OrderByDescending(model => model.DateOfCreation)
                    .Skip(skipCount)
                    .Take(takeCount)
                    .ToList();
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(int userId, int albumId, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userId, unitOfWork);

                if (_secureService.CanUserViewPhotos(user.Id, albumId))
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


        public IEnumerable<PhotoModel> GetPhotos(int userId, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                // todo: create a criterions for grabing all user albums' IDs and check every for access permissions
                return unitOfWork.Photos.Filter(model => model.OwnerId == userId)
                    .Where(model => !model.IsDeleted)
                    .OrderByDescending(model => model.DateOfCreation)
                    .ThenBy(model => model.Id)
                    .Skip(skipCount)
                    .Take(takeCount)
                    .ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="photoId"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        /// todo: Needs realization
        public IEnumerable<PhotoModel> GetPhotosByTags(int userId, int photoId, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userId, unitOfWork);
                var photo = GetPhoto(userId, photoId);

                foreach(var photoTag in photo.PhotoTags)
                { 
                    
                }




                return null;
            }
        }

        public PhotoModel GetPhoto(int userId, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                PhotoModel photoModel = unitOfWork.Photos.Find(photoId);

                if (_secureService.CanUserViewPhotos(userId, photoModel.AlbumId))
                {
                    return photoModel;
                }

                throw new NoEnoughPrivilegesException("User can't get access to photos");
            }
        }


        //todo Shall refactor and delete. Soon ..)
        public PhotoModel GetPhotoWithoutRightsCheck(int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                PhotoModel photoModel = unitOfWork.Photos.Find(photoId);
                return photoModel;
            }
        }


        public IEnumerable<UserModel> GetLikes(int userId, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                PhotoModel photo = unitOfWork.Photos.Find(photoId);

                if (_secureService.CanUserViewLikes(userId, photo.AlbumId))
                {
                    return photo.Likes.ToList();
                }

                throw new NoEnoughPrivilegesException("User can't get access to photoModel's likes");
            }
        }

        public void AddLike(int userId, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userId, unitOfWork);

                unitOfWork.Photos.Find(photoId).Likes.Add(user);
                unitOfWork.SaveChanges();

                _eventsAggregator.PushLikeToPhotoAddedEvent(user, photoId);
            }
        }

        public IEnumerable<PhotoModel> GetPublicPhotos(int userId, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IEnumerable<AlbumModel> avialableAlbums = _secureService.GetAvailableAlbums(userId, unitOfWork);

                return
                    avialableAlbums.SelectMany(model => model.Photos)
                        .OrderByDescending(model => model.DateOfCreation)
                        .ThenBy(photo => photo.Id)
                        .Skip(skipCount)
                        .Take(takeCount)
                        .ToList();
            }
        }

        public IEnumerable<PhotoModel> GetRandomPublicPhotos(int takeCount)
        {
            return PublicPhotos.Values.OrderBy(x => Guid.NewGuid())
                                       .Take(takeCount)
                                       .ToList();
        }
    }
}