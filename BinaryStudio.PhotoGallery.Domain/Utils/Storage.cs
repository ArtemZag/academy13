using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Utils
{
    internal class Storage : IStorage
    {
        private readonly IPathUtil pathUtil;

        public Storage(IPathUtil pathUtil)
        {
            this.pathUtil = pathUtil;
        }

        public string GetAlbumPath(AlbumModel album, IUnitOfWork unitOfWork)
        {
            UserModel user = unitOfWork.Users.Find(album.OwnerId);

            return pathUtil.BuildAbsoluteAlbumPath(user.Id, album.Id);
        }

        public string GetOriginalPhotoPath(PhotoModel photo, IUnitOfWork unitOfWork)
        {
            AlbumModel album = unitOfWork.Albums.Find(photo.AlbumModelId);
            UserModel user = unitOfWork.Users.Find(album.OwnerId);

            return pathUtil.BuildAbsoluteOriginalPhotoPath(user.Id, album.Id, photo.Id, photo.Format);
        }

        public IEnumerable<string> GetThumnailsPaths(PhotoModel photo, IUnitOfWork unitOfWork)
        {
            AlbumModel album = unitOfWork.Albums.Find(photo.AlbumModelId);
            UserModel user = unitOfWork.Users.Find(album.OwnerId);

            var result = new List<string>
            {
                pathUtil.BuildAbsoluteThumbailPath(user.Id, album.Id, photo.Id, photo.Format, ImageSize.Big),
                pathUtil.BuildAbsoluteThumbailPath(user.Id, album.Id, photo.Id, photo.Format, ImageSize.Medium),
                pathUtil.BuildAbsoluteThumbailPath(user.Id, album.Id, photo.Id, photo.Format, ImageSize.Small)
            };

            return result;
        }
    }
}