using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Utils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services.Tasks
{
    // todo: test! 
    internal class CleanupTask : DbService, ICleanupTask
    {
        private readonly IStorage storage;

        public CleanupTask(IUnitOfWorkFactory workFactory, IStorage storage)
            : base(workFactory)
        {
            this.storage = storage;
        }

        public void Execute()
        {
            try
            {
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    CleanPhotos(unitOfWork);
                    CleanAlbums(unitOfWork);
                }
            }
            catch (Exception e)
            {
                throw new CleanupException(e);
            }
        }

        private void CleanPhotos(IUnitOfWork unitOfWork)
        {
            List<PhotoModel> photosToCleanup = GetPhotosForCleanup(unitOfWork);

            photosToCleanup.ForEach(model => CleanPhoto(model, unitOfWork));
            CleanDb(photosToCleanup, unitOfWork);
        }

        private List<PhotoModel> GetPhotosForCleanup(IUnitOfWork unitOfWork)
        {
            return unitOfWork.Photos.Filter(model => model.IsDeleted).ToList();
        }

        private void CleanPhoto(PhotoModel photo, IUnitOfWork unitOfWork)
        {
            CleanOriginal(photo, unitOfWork);
            CleanThumbnails(photo, unitOfWork);
        }

        private void CleanOriginal(PhotoModel photo, IUnitOfWork unitOfWork)
        {
            string path = storage.GetOriginalPhotoPath(photo, unitOfWork);

            DeleteFile(path);
        }

        private void CleanThumbnails(PhotoModel photo, IUnitOfWork unitOfWork)
        {
            IEnumerable<string> thumbnailPaths = storage.GetThumnailsPaths(photo, unitOfWork);

            foreach (string currentThumbnail in thumbnailPaths)
            {
                DeleteFile(currentThumbnail);
            }
        }

        private void CleanAlbums(IUnitOfWork unitOfWork)
        {
            List<AlbumModel> albumsToCleanup = GetAlbumsForCleanup();

            albumsToCleanup.ForEach(model => CleanAlbum(model, unitOfWork));

            CleanDb(albumsToCleanup, unitOfWork);
        }

        private List<AlbumModel> GetAlbumsForCleanup()
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Albums.Filter(model => model.IsDeleted).ToList();
            }
        }

        private void CleanAlbum(AlbumModel album, IUnitOfWork unitOfWork)
        {
            string albumPath = storage.GetAlbumPath(album, unitOfWork);

            Directory.Delete(albumPath, true);
        }

        private void CleanDb(IEnumerable<AlbumModel> albumsToCleanup, IUnitOfWork unitOfWork)
        {
            foreach (AlbumModel albumModel in albumsToCleanup)
            {
                unitOfWork.Albums.Delete(albumModel);
            }
        }

        private void CleanDb(IEnumerable<PhotoModel> photosToCleanup, IUnitOfWork unitOfWork)
        {
            foreach (PhotoModel photoModel in photosToCleanup)
            {
                unitOfWork.Photos.Delete(photoModel);
            }
        }

        private void DeleteFile(string path)
        {
            File.Delete(path);
        }
    }
}