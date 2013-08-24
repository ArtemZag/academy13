using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class ResizePhotoService : DbService, IResizePhotoService
    {
        private readonly ISecureService _secureService;
        private readonly IPathUtil _util;

        public ResizePhotoService(IUnitOfWorkFactory workFactory, ISecureService secureService, IPathUtil util)
            : base(workFactory)
        {
            _secureService = secureService;
            _util = util;
        }

        public string GetUserAvatar(int userId, ImageSize size)
        {
            using (IUnitOfWork unit = WorkFactory.GetUnitOfWork())
            {
                if (unit.Users.Contains(user => user.Id == userId))
                {
                    var processor = new PhotoProcessor(userId, _util);
                    return processor.GetUserAvatar(size);
                }
                throw new UserNotFoundException(
                    string.Format("Error loading the user's avatar. User with ID {0} not found", userId));
            }
        }

        public IEnumerable<string> GetUserAlbumThumbnails(int userId, int albumId)
        {
            using (IUnitOfWork unit = WorkFactory.GetUnitOfWork())
            {
                if (_secureService.CanUserViewPhotos(userId, albumId))
                {
                    var processor = new PhotoProcessor(userId, albumId, 64, _util);
                    List<PhotoModel> photos =
                        unit.Photos.Filter(
                            photo => photo.OwnerId == userId && photo.AlbumId == albumId && !photo.IsDeleted).ToList();

                    return processor.GetThumbnails();
                }

                throw new AccessViolationException(
                    string.Format("User with ID {0} dont have rights to access thumbnails", userId));
            }
        }

        //Высота колажа = heightOfOneLineInTheCollage*numberOfLines
        public string GetCollage(int userId, int albumId, int collageWidth, int heightOfOneLineInTheCollage,
            int numberOfLines)
        {
            using (IUnitOfWork unit = WorkFactory.GetUnitOfWork())
            {
                if (_secureService.CanUserViewPhotos(userId, albumId))
                {
                    var processor = new PhotoProcessor(userId, albumId, heightOfOneLineInTheCollage, _util);
                    List<PhotoModel> photos =
                        unit.Photos.Filter(
                            photo => photo.OwnerId == userId && photo.AlbumId == albumId && !photo.IsDeleted).ToList();

                    return processor.CreateCollageIfNotExist(collageWidth, numberOfLines);
                }

                throw new AccessViolationException(
                    string.Format("User with ID {0} dont have rights to access thumbnails", userId));
            }
        }

        public IEnumerable<PhotoModel> GetAvailablePhotos(int userId, int albumId)
        {
            using (IUnitOfWork unit = WorkFactory.GetUnitOfWork())
            {
                if (_secureService.CanUserViewPhotos(userId, albumId))
                    return
                        unit.Photos.Filter(
                            photo => photo.OwnerId == userId && photo.AlbumId == albumId && !photo.IsDeleted).ToList();

                throw new AccessViolationException(
                    string.Format("User with ID {0} dont have rights to access thumbnails", userId));
            }
        }

        public string GetThumbnail(int userId, int albumId, PhotoModel model, int maxHeight)
        {
            if (_secureService.CanUserViewPhotos(userId, albumId))
            {
                var processor = new PhotoProcessor(userId, albumId, maxHeight, _util);
                return processor.CreateThumbnail(userId, albumId, model, maxHeight);
            }
            throw new AccessViolationException(
                string.Format("User with ID {0} dont have rights to access thumbnail", userId));
        }
    }
}