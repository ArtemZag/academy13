using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        private readonly List<AlbumModel> systemAlbumsList = new List<AlbumModel>()
            {
                #region "Temporary" album
                new AlbumModel
                {
                    AlbumName = "Temporary",
                    Description = "System album. Not for use",
                    OwnerId = -1,
                    IsDeleted = false,
                    Permissions = 11111,
                    Photos = new Collection<PhotoModel>(),
                    AvailableGroups = new Collection<AvailableGroupModel>(),
                    AlbumTags = new Collection<AlbumTagModel>()
                }
                 #endregion
            };

        public AlbumModel CreateAlbum(int userId, AlbumModel albumModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userId, unitOfWork);

                if (user.Albums.ToList().Find(album => album.AlbumName == albumModel.AlbumName) != null)
                {
                    throw new AlbumAlreadyExistException(
                        string.Format("Can't create album \"{0}\" because it is already exist", albumModel.AlbumName));
                }

                user.Albums.Add(albumModel);

                unitOfWork.SaveChanges();

                return albumModel;
            }
        }

        public AlbumModel CreateAlbum(string userEmail, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                AlbumModel albumModel = CreateAlbum(user.Id, albumName);

                return albumModel;
            }
        }

        public AlbumModel CreateAlbum(int userId, string albumName)
        {
            var albumModel = new AlbumModel
                {
                    AlbumName = albumName,
                    OwnerId = userId
                };

            albumModel = CreateAlbum(userId, albumModel);

            return albumModel;
        }

        public void CreateSystemAlbums(int userId)
        {
            foreach (var systemAlbum in systemAlbumsList)
            {
                systemAlbum.OwnerId = userId;
                CreateAlbum(userId, systemAlbum);
            }
        }

        private bool IsAlbumSystem(AlbumModel album)
        {
            return systemAlbumsList.Find(systemAlbum => systemAlbum.AlbumName == album.AlbumName) != null;
        }

        public void DeleteAlbum(string userEmail, int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumId);

                
                if (IsAlbumSystem(album))
                {
                    throw new AlbumNotFoundException(string.Format("Can't delete system album with id={0}", albumId));
                }

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

                throw new AlbumNotFoundException(albumId.ToString());
            }
        }

        public int GetAlbumId(int userId, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel foundAlbum =
                    unitOfWork.Albums.Find(album => album.AlbumName == albumName && album.OwnerId == userId);

                if (foundAlbum == null)
                {
                    throw new AlbumNotFoundException(albumName);
                }

                return foundAlbum.Id;
            }
        }

        public int GetAlbumId(string userEmail, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel foundUser = unitOfWork.Users.Find(user => user.Email == userEmail);

                return GetAlbumId(foundUser.Id, albumName);
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