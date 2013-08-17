using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class AlbumService : DbService, IAlbumService
    {
        private readonly ISecureService _secureService;

        public AlbumService(IUnitOfWorkFactory workFactory, ISecureService secureService) : base(workFactory)
        {
            _secureService = secureService;
        }

        public void CreateAlbum(string userEmail, AlbumModel albumModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                if (user.Albums.ToList().Find(album => album.AlbumName == albumModel.AlbumName) != null)
                {
                    throw new AlbumAlreadyExistException(albumModel.AlbumName);
                }

                user.Albums.Add(albumModel);

                unitOfWork.SaveChanges();
            }
        }

        public void CreateAlbum(string userEmail, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                var albumModel = new AlbumModel
                {
                    AlbumName = albumName,
                    OwnerId = user.Id
                };

                try
                {
                    CreateAlbum(userEmail, albumModel);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        public void DeleteAlbum(string userEmail, int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumId);

                album.IsDeleted = true;

                unitOfWork.SaveChanges();
            }
        }

        public AlbumModel GetAlbum(int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return GetAlbum(albumId, unitOfWork);
            }
        }

        public IEnumerable<AlbumModel> GetAllAlbums(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                return user.Albums.Where(model => !model.IsDeleted).ToList();
            }
        }

        public AlbumModel GetAlbum(string userEmail, int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                AlbumModel album = GetAlbum(albumId, unitOfWork);

                if (album.OwnerId == user.Id)
                {
                    return album;
                }

                throw new AlbumNotFoundException();
            }
        }

        public int GetAlbumId(string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel foundAlbum = unitOfWork.Albums.Find(album => album.AlbumName == albumName);

                if (foundAlbum == null)
                {
                    throw new AlbumNotFoundException();
                }

                return foundAlbum.Id;
            }
        }

        public bool IsExist(string userEmail, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel foundUser = unitOfWork.Users.Find(user => user.Email == userEmail);

                AlbumModel foundAlbum =
                    unitOfWork.Albums.Find(album => album.AlbumName == albumName && album.OwnerId == foundUser.Id);

                return foundAlbum != null;
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return GetAvailableAlbums(userId, unitOfWork);
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel foundUser = unitOfWork.Users.Find(user => user.Email == userEmail);
                return GetAvailableAlbums(foundUser.Id, unitOfWork);
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork)
        {
            return _secureService.GetAvailableAlbums(userId, unitOfWork);
        }
    }
}