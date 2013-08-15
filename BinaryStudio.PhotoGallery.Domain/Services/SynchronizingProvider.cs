using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

[assembly: InternalsVisibleTo("BinaryStudio.PhotoGallery.Core.PhotoUtils")]
namespace BinaryStudio.PhotoGallery.Domain.Services
{
    class SynchronizingProvider: DbService
    {
        public SynchronizingProvider(IUnitOfWorkFactory workFactory) : base(workFactory) { }

        public List<AlbumModel> GetAlbumsForSyncronize()
        {
            List<AlbumModel> models;
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                models = unitOfWork.Albums.Filter(model => !model.IsDeleted).ToList();
            }
            return models;
        }

        public List<UserModel> GetOwnersOfAlbums(IEnumerable<AlbumModel> iAlbumModels)
        {
            var users = new List<UserModel>();
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                foreach (var iAlbumModel in iAlbumModels)
                {
                    users.Add(unitOfWork.Users.Find(model => model.Id == iAlbumModel.UserModelId));
                }               
            }
            return users;
        }

        public List<PhotoModel> GetPhotosForSyncronizeByModelAndUserId(int userId, int albumId)
        {
            List<PhotoModel> models;
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                models =
                    unitOfWork.Photos.Filter(
                        photo => !photo.IsDeleted && photo.AlbumModelId == albumId && photo.UserModelId == userId)
                              .ToList();
            }
            return models;
        }
        public List<PhotoModel> GetDeletedPhotosByModelAndUserId(int userId, int albumId)
        {
            List<PhotoModel> models;
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                models =
                    unitOfWork.Photos.Filter(
                        photo => photo.IsDeleted && photo.AlbumModelId == albumId && photo.UserModelId == userId)
                            .ToList();
            }
            return models;
        }
    }
}
