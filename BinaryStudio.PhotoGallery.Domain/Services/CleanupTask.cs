using System;
using System.Collections.Generic;
using System.IO;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Utils;
using BinaryStudio.PhotoGallery.Models;
using System.Linq;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class CleanupTask : DbService, ICleanupTask
    {
        private IStorage storage;

        public IStorage Storage { set { storage = value; }}

        public CleanupTask(IUnitOfWorkFactory workFactory, IStorage storage) : base(workFactory)
        {
            this.storage = storage;
        }

        public void Execute()
        {
            try
            {
                CleanPhotos();
                CleanAlbums();
            }
            catch (Exception e)
            {
                throw new CleanupException(e);
            }
        }

        private void CleanPhotos()
        {
            List<PhotoModel> photosToCleanup = GetPhotosForCleanup();

            photosToCleanup.ForEach(CleanPhoto);
            DeleteDbRows(photosToCleanup);

            CleanTemporaryPhotos();
        }

        private List<PhotoModel> GetPhotosForCleanup()
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Photos.Filter(model => model.IsDeleted).ToList();
            }
        }

        private void CleanPhoto(PhotoModel photo)
        {
            CleanOriginal(photo);
            CleanThumbnails(photo);
        }

        private void CleanTemporaryPhotos()
        {
            IEnumerable<string> temporaryPhotosDirectories = storage.GetTemporaryDirectoriesPathes();

            foreach (var temporaryDirectoryPath in temporaryPhotosDirectories)
            {
                var directory = new DirectoryInfo(temporaryDirectoryPath);

                DeleteFiles(directory);
            }
        }

        private void CleanOriginal(PhotoModel photo)
        {
            string path = storage.GetOriginalPhotoPath(photo);

            DeleteFile(path);
        }

        private void CleanThumbnails(PhotoModel photo)
        {
            IEnumerable<string> thumbnailDirectories = storage.GetThumbnailDirectoryPath(photo);

            foreach (var formatDirectory in thumbnailDirectories)
            {
                string thumbnailPath = Path.Combine(formatDirectory, photo.Id + photo.Format);

                DeleteFile(thumbnailPath);
            }
        }

        private void CleanAlbums()
        {
            List<AlbumModel> albumsToCleanup = GetAlbumsForCleanup();

            albumsToCleanup.ForEach(CleanAlbum);

            DeleteDbRows(albumsToCleanup);
        }

        private List<AlbumModel> GetAlbumsForCleanup()
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Albums.Filter(model => model.IsDeleted).ToList();
            }
        } 

        private void CleanAlbum(AlbumModel album)
        {
            string albumPath = storage.GetAlbumPath(album);

            Directory.Delete(albumPath, true);
        }

        private void DeleteDbRows(IEnumerable<AlbumModel> albumsToCleanup)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                foreach (var albumModel in albumsToCleanup)
                {
                    unitOfWork.Albums.Delete(albumModel);
                }
            }
        }

        private void DeleteDbRows(IEnumerable<PhotoModel> photosToCleanup)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                foreach (var photoModel in photosToCleanup)
                {
                    unitOfWork.Photos.Delete(photoModel);
                }
            }
        }

        private void DeleteFiles(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }
        }

        private void DeleteFile(string path)
        {
            File.Delete(path);
        }
    }
}