﻿using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoService : DbService, IPhotoService
    {
        private readonly ISecureService _secureService;

        public PhotoService(IUnitOfWorkFactory workFactory, ISecureService secureService) : base(workFactory)
        {
            _secureService = secureService;
        }

        public PhotoModel AddPhoto(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(photo.OwnerId, photo.OwnerId, unitOfWork);

                album.Photos.Add(photo);

                unitOfWork.SaveChanges();

                return photo;
            }
        }

        public PhotoModel AddPhoto(string userEmail, string albumName, PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                if (_secureService.CanUserAddPhoto(user.Id, album.Id))
                {
                    photo.OwnerId = user.Id;
                    album.Photos.Add(photo);

                    unitOfWork.SaveChanges();

                    return photo;
                }

                throw new NoEnoughPrivileges("User can't get access to photos", null);
            }
        }

        public IEnumerable<int> AddPhotos(string userEmail, string albumName, IEnumerable<PhotoModel> photos)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                if (_secureService.CanUserAddPhoto(user.Id, album.Id))
                {
                    IEnumerable<PhotoModel> photoModels = photos as IList<PhotoModel> ?? photos.ToList();
                    foreach (PhotoModel photo in photoModels)
                    {
                        album.Photos.Add(photo);
                    }
                    unitOfWork.SaveChanges();

                    return photoModels.Select(photo => photo.Id).ToList();
                }

                throw new NoEnoughPrivileges("User can't get access to photos", null);
            }
        }

        public void DeletePhoto(string userEmail, PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                if (_secureService.CanUserDeletePhoto(user.Id, photo.Id))
                {
                    PhotoModel photoToDelete = unitOfWork.Photos.Find(model => model.Id == photo.Id);
                    photoToDelete.IsDeleted = true;

                    unitOfWork.SaveChanges();
                }
                else
                {
                    throw new NoEnoughPrivileges("User can't get access to photos", null);
                }
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, string albumName, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                if (_secureService.CanUserViewPhotos(user.Id, album.Id))
                {
                    return album.Photos.OrderBy(model => model.DateOfCreation)
                                .ThenBy(model => model.Id)
                                .Skip(skipCount)
                                .Take(takeCount)
                                .ToList();
                }

                throw new NoEnoughPrivileges("User can't get access to photos", null);
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, int albumId, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                if (_secureService.CanUserViewPhotos(user.Id, albumId))
                {
                    return unitOfWork.Photos.Filter(model => model.AlbumId == albumId)
                                     .Where(model => !model.IsDeleted)
                                     .OrderBy(model => model.DateOfCreation)
                                     .ThenBy(model => model.Id)
                                     .Skip(skipCount)
                                     .Take(takeCount)
                                     .ToList();
                }

                throw new NoEnoughPrivileges("User can't get access to photos", null);
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
                                 .OrderBy(model => model.DateOfCreation)
                                 .ThenBy(model => model.Id)
                                 .Skip(skipCount)
                                 .Take(takeCount)
                                 .ToList();
            }
        }

        public PhotoModel GetPhoto(string userEmail, int photoID)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                PhotoModel photo = unitOfWork.Photos.Find(photoID);

                if (_secureService.CanUserViewPhotos(user.Id, photo.AlbumId))
                {
                    return photo;
                }

                throw new NoEnoughPrivileges("User can't get access to photos", null);
            }
        }

        public IEnumerable<UserModel> GetLikes(string userEmail, int photoID)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                PhotoModel photo = unitOfWork.Photos.Find(photoID);

                if (_secureService.CanUserViewLikes(user.Id, photo.AlbumId))
                {
                    return photo.Likes.ToList();
                }

                throw new NoEnoughPrivileges("User can't get access to photo's likes", null);
            }
        }

        public void AddLike(string userEmail, int photoID)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                unitOfWork.Photos.Find(photoID).Likes.Add(user);
                unitOfWork.SaveChanges();
            }
        }

        public IEnumerable<PhotoModel> GetPublicPhotos(string userEmail, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                var allAvailableAlbums =
                    (IEnumerable<PhotoModel>) _secureService.GetAvailableAlbums(user.Id, unitOfWork)
                                                            .OrderBy(album => album.DateOfCreation)
                                                            .Select(
                                                                album =>
                                                                unitOfWork.Photos.Filter(
                                                                    photo => photo.AlbumId == album.Id)
                                                                          .Where(photo => !photo.IsDeleted));

                return allAvailableAlbums.OrderBy(photo => photo.DateOfCreation)
                                         .ThenBy(photo => photo.Id)
                                         .Skip(skipCount)
                                         .Take(takeCount);
            }
        }
    }
}