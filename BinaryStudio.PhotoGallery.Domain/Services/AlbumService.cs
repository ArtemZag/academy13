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

        public void CreateAlbum(string userEmail, AlbumModel album)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                user.Albums.Add(album);

                unitOfWork.SaveChanges();
            }
        }

        public void CreateAlbum(string userEmail, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                var album = new AlbumModel
                {
                    AlbumName = albumName,
                    UserId = user.Id
                };

                user.Albums.Add(album);

                unitOfWork.SaveChanges();
            }
        }

        public void DeleteAlbum(string userEmail, int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumId, unitOfWork);

                album.IsDeleted = true;

                unitOfWork.SaveChanges();
            }
        }

        public AlbumModel GetAlbum(int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Albums.Find(albumId);
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

                AlbumModel album = GetAlbum(albumId);

                if (album.UserId == user.Id)
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
                var foundAlbum = unitOfWork.Albums.Find(album => album.AlbumName == albumName);

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
                var foundUser = unitOfWork.Users.Find(user => user.Email == userEmail);

                var foundAlbum = unitOfWork.Albums.Find(album => album.AlbumName == albumName && album.UserId == foundUser.Id);

                return foundAlbum != null;
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return this.GetAvailableAlbums(userId, unitOfWork);
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var foundUser = unitOfWork.Users.Find(user => user.Email == userEmail);
                return this.GetAvailableAlbums(foundUser.Id, unitOfWork);
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork)
        {
            return _secureService.GetAvailableAlbums(userId, unitOfWork);
        }
    }
}