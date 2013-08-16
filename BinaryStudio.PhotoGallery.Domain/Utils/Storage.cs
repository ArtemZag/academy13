using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Utils
{
    internal class Storage : IStorage
    {
        private readonly IPathUtil pathUtil;
        private readonly IUnitOfWorkFactory workFactory;

        public Storage(IUnitOfWorkFactory workFactory, IPathUtil pathUtil)
        {
            this.workFactory = workFactory;
            this.pathUtil = pathUtil;
        }

        public string GetAlbumPath(AlbumModel album)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(album.UserId, unitOfWork);

                return pathUtil.BuildAlbumPath(user.Id, album.Id);
            }
        }

        public string GetAlbumPath(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                AlbumModel album = unitOfWork.Albums.Find(model => model.Id == photo.AlbumId);
                UserModel user = unitOfWork.Users.Find(model => model.Id == album.UserId);

                return pathUtil.BuildAlbumPath(user.Id, album.Id);
            }
        }

        public string GetOriginalPhotoPath(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(photo.AlbumId, unitOfWork);
                UserModel user = GetUser(album.UserId, unitOfWork);

                return pathUtil.BuildOriginalPhotoPath(user.Id, album.Id, photo.Id, photo.Format);
            }
        }

        public IEnumerable<string> GetThumnailsPaths(PhotoModel photo)
        {
            var result = new Collection<string>();

            string thumbnailsDirectoryPath = GetThumbnailsDirectoryPath(photo);

            IEnumerable<string> thumnailFormatsPaths = Directory.EnumerateDirectories(thumbnailsDirectoryPath);

            foreach (string thumbnailFormatPath in thumnailFormatsPaths)
            {
                string currentThumbnail = Path.Combine(thumbnailFormatPath, photo.Id + photo.Format);

                if (File.Exists(currentThumbnail))
                {
                    result.Add(currentThumbnail);
                }
            }

            return result;
        }

        public IEnumerable<string> GetTemporaryDirectoriesPaths()
        {
            return pathUtil.BuildTemporaryDirectoriesPaths();
        }

        private string GetThumbnailsDirectoryPath(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(photo.AlbumId, unitOfWork);
                UserModel user = GetUser(album.UserId, unitOfWork);

                return pathUtil.BuildThumbnailsPath(user.Id, album.Id);
            }
        }

        private UserModel GetUser(int userId, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Users.Find(model => model.Id == userId);
        }

        private AlbumModel GetAlbum(int albumId, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Albums.Find(model => model.Id == albumId && !model.IsDeleted);
        }
    }
}