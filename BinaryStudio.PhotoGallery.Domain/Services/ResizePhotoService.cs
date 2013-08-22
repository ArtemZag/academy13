using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;
using Microsoft.Practices.Unity;

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
        public string GetUserAvatar(int userId,AvatarSize size)
        {
            using (var unit = WorkFactory.GetUnitOfWork())
            {
                if (unit.Users.Contains(user => user.Id == userId))
                {
                    var processor = new AsyncPhotoProcessor(userId,_util);
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
                if (_secureService.GetAvailableAlbums(userId, unit)
                                  .Select(model => model).Any(model => model.OwnerId == userId))
                {
                    var processor = new AsyncPhotoProcessor(userId, albumId, 64, _util);
                    return processor.GetThumbnails();
                }
             
                    throw new AccessViolationException(
                        string.Format("User with ID {0} dont have rights to access thumbnails", userId));
            }
        }

        public string GetCollage(int userId, int albumId)
        {
            throw new NotImplementedException();
        }

        public string GetThumbnail(int userId, int albumId, int photoId)
        {
            throw new NotImplementedException();
        }
    }
}
