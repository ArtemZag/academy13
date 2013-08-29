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
        private const string TEMPORARY_ALBUM_NAME = "Temporary";

        private readonly List<AlbumModel> _systemAlbumsList = new List<AlbumModel>
        {
            #region "Temporary" album
            new AlbumModel
            {
                Name = TEMPORARY_ALBUM_NAME,
                Description = "System album. Not for use",
                OwnerId = -1,
                IsDeleted = false,
                Permissions = 0,
                Photos = new Collection<PhotoModel>(),
                AvailableGroups = new Collection<AvailableGroupModel>(),
            }
            #endregion
        };

        private readonly ISecureService _secureService;

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

        public IEnumerable<AlbumModel> GetAvialableAlbums(int userId, IUnitOfWork unitOfWork)
        {
            var result = new List<AlbumModel>();

            IEnumerable<AlbumModel> publicAlbums = _secureService.GetPublicAlbums(userId, unitOfWork);
            IEnumerable<AlbumModel> userAlbums = GetAllAlbums(userId, unitOfWork);

            IEnumerable<AlbumModel> difference =
                userAlbums.Where(model => publicAlbums.All(albumModel => albumModel.Id != model.Id));

            result.AddRange(publicAlbums);
            result.AddRange(difference);

            return result;
        }

        public IEnumerable<string> GetAlbumsTags(int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return
                    unitOfWork.Albums.Find(albumId)
                        .Photos.SelectMany(model => model.Tags)
                        .Select(model => model.TagName)
                        .Distinct()
                        .ToList();
            }
        }

        public int GetPhotosCount(int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel album = unitOfWork.Albums.Find(albumId);

                return album.Photos.Count;
            }
        }

        public AlbumModel GetAlbum(int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return GetAlbum(albumId, unitOfWork);
            }
        }

        public int AlbumsCount(int userId,int tempAlbumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return
                    unitOfWork.Albums.Filter(
                        model => model.OwnerId == userId && !model.IsDeleted && model.Id != tempAlbumId).Count();
            }
        }

        public IEnumerable<AlbumModel> GetAlbumsRange(int userRequestsId, int userOwnerId, int skipCount, int takeCount, out bool reasonOfNotAlbums)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                reasonOfNotAlbums = true;
                var albums = unitOfWork.Albums.Filter(model => model.OwnerId == userOwnerId && !model.IsDeleted && model.Name != TEMPORARY_ALBUM_NAME)
                                 .OrderByDescending(model => model.DateOfCreation)
                                 .Skip(skipCount)
                                 .Take(takeCount)
                                 .ToList();
                if (albums.Count == 0)
                    reasonOfNotAlbums = false;

                var albumsToTake = 
                    albums.Select(album => album)
                          .Where(album => _secureService.CanUserViewPhotos(userRequestsId, album.Id));

                if (albumsToTake.Count() == 0)
                    reasonOfNotAlbums = true;

                return albumsToTake;
            }
        }

        public void UpdateAlbum(AlbumModel albumModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Albums.Update(albumModel);

                unitOfWork.SaveChanges();
            }
        }

        public IEnumerable<AlbumModel> GetAllAlbums(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return GetAllAlbums(userId, unitOfWork);
            }
        }

        public IEnumerable<AlbumModel> GetAllAlbums(int userId, IUnitOfWork unitOfWork)
        {
            UserModel user = GetUser(userId, unitOfWork);

            return user.Albums.Where(model => !model.Name.Equals(TEMPORARY_ALBUM_NAME) && !model.IsDeleted).ToList();
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

        private bool IsAlbumSystem(AlbumModel album)
        {
            return _systemAlbumsList.Find(systemAlbum => systemAlbum.Name == album.Name) != null;
        }
    }
}