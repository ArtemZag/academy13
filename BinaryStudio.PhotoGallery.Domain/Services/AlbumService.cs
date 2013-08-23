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


        private readonly List<AlbumModel> _systemAlbumsList = new List<AlbumModel>
            {
                #region "Temporary" album
                new AlbumModel
                    {
                        Name = "Temporary",
                        Description = "System album. Not for use",
                        OwnerId = -1,
                        IsDeleted = false,
                        Permissions = 11111,
                        Photos = new Collection<PhotoModel>(),
                        AvailableGroups = new Collection<AvailableGroupModel>(),
                        Tags = new Collection<AlbumTagModel>()
                    }
                #endregion
            };

        public AlbumService(IUnitOfWorkFactory workFactory, ISecureService secureService) : base(workFactory)
        {
            _secureService = secureService;
        }

        public AlbumModel CreateAlbum(int userId, AlbumModel albumModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userId, unitOfWork);

                if (user.Albums.ToList().Find(album => album.Name == albumModel.Name) != null)
                {
                    throw new AlbumAlreadyExistException(
                        string.Format("Can't create album \"{0}\" because it is already exist", albumModel.Name));
                }

                user.Albums.Add(albumModel);

                unitOfWork.SaveChanges();

                return albumModel;
            }
        }

        public AlbumModel CreateAlbum(int userId, string albumName)
        {
            var albumModel = new AlbumModel
                {
                    Name = albumName,
                    OwnerId = userId
                };

            albumModel = CreateAlbum(userId, albumModel);

            return albumModel;
        }

        public void CreateSystemAlbums(int userId)
        {
            foreach (AlbumModel systemAlbum in _systemAlbumsList)
            {
                systemAlbum.OwnerId = userId;
                CreateAlbum(userId, systemAlbum);
            }
        }

        public void DeleteAlbum(int userId, int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userId, unitOfWork);
                AlbumModel album = GetAlbum(userId, albumId, unitOfWork);

                if (IsAlbumSystem(album))
                {
                    throw new AlbumNotFoundException(string.Format("Can't delete system album with id={0}", albumId));
                }

                if (album.OwnerId != userId && !user.IsAdmin)
                {
                    throw new AlbumNotFoundException(
                        string.Format("You have no permissions for deleting album with id={0}", albumId));
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

        public int AlbumsCount(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Albums.Filter(model => model.OwnerId == userId && !model.IsDeleted).Sum(model => 1);
            }
        }

        public IEnumerable<AlbumModel> GetAlbumsRange(int userId, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Albums
                                 .Filter(model => model.OwnerId == userId && !model.IsDeleted)
                                 .OrderByDescending(model => model.DateOfCreation)
                                 .Skip(skipCount)
                                 .Take(takeCount)
                                 .ToList();
            }
        }

        public IEnumerable<AlbumTagModel> GetTags(int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.AlbumTags.Filter(tag => tag.Id == albumId).ToList();
            }
        }

        public IEnumerable<AlbumModel> GetAllAlbums(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userId, unitOfWork);

                return user.Albums.Where(model => !model.IsDeleted).ToList();
            }
        }


        public int GetAlbumId(int userId, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel foundAlbum =
                    unitOfWork.Albums.Find(album => album.Name == albumName && album.OwnerId == userId);

                if (foundAlbum == null)
                {
                    throw new AlbumNotFoundException(string.Format("Can't find album with name={0}", albumName));
                }

                return foundAlbum.Id;
            }
        }

        public bool IsExist(int userId, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel userModel = GetUser(userId, unitOfWork);

                if (userModel == null)
                {
                    throw new UserNotFoundException(string.Format("Can't find user with id={0}", userId));
                }

                AlbumModel albumModel =
                    unitOfWork.Albums.Find(album => album.Name == albumName && album.OwnerId == userModel.Id);

                return albumModel != null;

            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return GetAvailableAlbums(userId, unitOfWork);
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork)
        {
            return _secureService.GetAvailableAlbums(userId, unitOfWork);
        }

        private bool IsAlbumSystem(AlbumModel album)
        {
            return _systemAlbumsList.Find(systemAlbum => systemAlbum.Name == album.Name) != null;
        }
    }
}