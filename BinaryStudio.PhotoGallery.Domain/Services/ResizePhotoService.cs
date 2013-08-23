using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    class ResizePhotoService : DbService, IResizePhotoService
    {
        private readonly ISecureService _secureService;
        private readonly IPathUtil _util;
        public ResizePhotoService(IUnitOfWorkFactory workFactory, ISecureService secureService, IPathUtil util)
            : base(workFactory)
        {
            _secureService = secureService;
            _util = util;
        }
        public string GetUserAvatar(int userId, AvatarSize size)
        {
            using (var unit = WorkFactory.GetUnitOfWork())
            {
                if (unit.Users.Contains(user => user.Id == userId))
                {
                    var processor = new AsyncPhotoProcessor(userId);
                    return processor.GetUserAvatar(size);
                }
                throw new UserNotFoundException(
                    string.Format("Error loading the user's avatar. User with ID {0} not found", userId));
            }
        }

        public IEnumerable<string> GetUserAlbumThumbnails(int userId, int albumId)
        {
            using (var unit = WorkFactory.GetUnitOfWork())
            {
                if (_secureService.CanUserViewPhotos(userId, albumId))
                {
                    var processor = new AsyncPhotoProcessor(userId, albumId, 64, _util);
                    return processor.GetThumbnails();
                }
             
                    throw new AccessViolationException(
                        string.Format("User with ID {0} dont have rights to access thumbnails", userId));
            }
        }

        //Высота колажа = heightOfOneLineInTheCollage*numberOfLines
        public string GetCollage(int userId, int albumId, int collageWidth, int heightOfOneLineInTheCollage, int numberOfLines)
        {
            using (var unit = WorkFactory.GetUnitOfWork())
            {
                if (_secureService.CanUserViewPhotos(userId, albumId))
                {
                    var processor = new AsyncPhotoProcessor(userId, albumId, heightOfOneLineInTheCollage, _util);
                    processor.SyncOriginalAndThumbnailImages();
                    return processor.CreateCollageIfNotExist(collageWidth, numberOfLines);
                }

                throw new AccessViolationException(
                    string.Format("User with ID {0} dont have rights to access thumbnails", userId));
            }
        }

        public string GetThumbnail(int userId, int albumId, string photoName, int maxHeight)
        {
            using (var unit = WorkFactory.GetUnitOfWork())
            {
                if (_secureService.CanUserViewPhotos(userId, albumId))
                {
                    if (unit.Photos.Contains(model => model.Name == photoName))
                    {
                        var processor = new AsyncPhotoProcessor(userId, albumId, maxHeight, _util);
                        processor.SyncOriginalAndThumbnailImages();
                        return _util.BuildPathToThumbnailFileOnServer(userId, albumId, maxHeight, photoName);
                    }

                    throw new Exception(string.Format("Photo with {0} in album {1} of user {2} not found",
                                                      photoName, albumId, userId));
                }
                throw new NoEnoughPrivilegesException(
                    string.Format("User with ID {0} dont have rights to access thumbnail", userId));
            }
        }
    }
}
