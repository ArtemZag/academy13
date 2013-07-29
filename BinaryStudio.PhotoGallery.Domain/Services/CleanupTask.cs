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
        private readonly IStorage storage;

        public CleanupTask(IUnitOfWorkFactory workFactory, IStorage storage) : base(workFactory)
        {
            this.storage = storage;
        }

        public void Execute()
        {
            CleanPhotos();
            CleanAlbums();
        }

        private void CleanPhotos()
        {
            List<PhotoModel> photosToCleanup;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                photosToCleanup = unitOfWork.Photos.Filter(model => model.IsDeleted).ToList();
            }

            photosToCleanup.ForEach(CleanPhoto);

            DeleteDbRows(photosToCleanup);

            CleanTemporaryPhotos();
        }

        private void CleanTemporaryPhotos()
        {
            IEnumerable<string> temporaryPhotosDirectories = storage.GetTemporaryDirectories();

            foreach (var temporaryDirectoryPath in temporaryPhotosDirectories)
            {
                var directory = new DirectoryInfo(temporaryDirectoryPath);

                foreach (var file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        private void CleanPhoto(PhotoModel photo)
        {
            CleanOriginal(photo);
            CleanThumbnails(photo);
        }

        private void CleanOriginal(PhotoModel photo)
        {
            string path = storage.GetOriginalPhotoPath(photo);

            DeleteFile(path);
        }

        private void CleanThumbnails(PhotoModel photo)
        {
            IEnumerable<string> thumbnailDirectories = storage.GetThumbnailFormatDirectories(photo);

            foreach (var thumbnailFormatDirectory in thumbnailDirectories)
            {
                string thumbnailPath = Path.Combine(thumbnailFormatDirectory, photo.PhotoName);

                DeleteFile(thumbnailPath);
            }
        }

        private void CleanAlbums()
        {
            List<AlbumModel> albumsToCleanup;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                albumsToCleanup = unitOfWork.Albums.Filter(model => model.IsDeleted).ToList();
            }

            albumsToCleanup.ForEach(CleanAlbum);

            DeleteDbRows(albumsToCleanup);
        }

        private void CleanAlbum(AlbumModel album)
        {
            string albumPath = storage.GetAlbumPath(album);

            DeleteFile(albumPath);
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

        private void DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                throw new CleanupException(e);
            }
        }
    }
}