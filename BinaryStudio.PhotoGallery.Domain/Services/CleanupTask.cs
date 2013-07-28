using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class CleanupTask : DbService, IPhotoCleanupTask
    {
        private readonly IPhotoService photoService;

        public CleanupTask(IUnitOfWorkFactory workFactory, IPhotoService photoService) : base(workFactory)
        {
            this.photoService = photoService;
        }

        public void Execute()
        {
            CleanPhotos();

            // albums
        }

        private void CleanPhotos()
        {
            List<PhotoModel> photosToCleanup;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                photosToCleanup = unitOfWork.Photos.Filter(model => model.IsDeleted).ToList();
            }

            photosToCleanup.ForEach(CleanPhoto);
        }

        private void CleanPhoto(PhotoModel photo)
        {
            CleanOriginal(photo);
            CleanThumbnails(photo);
            // from db
        }

        private void CleanOriginal(PhotoModel photo)
        {
            string path = photoService.GetOriginalPhotoPath(photo);

            DeletePhoto(path);
        }

        private void CleanThumbnails(PhotoModel photo)
        {
            string thumbnailsDirectoryPath = photoService.GetThumbnailsPath(photo);
            IEnumerable<string> thumbnailFormats = Directory.GetDirectories(thumbnailsDirectoryPath);

            foreach (var thumbnailFormatDirectory in thumbnailFormats)
            {
                string thumbnailPath = Path.Combine(thumbnailFormatDirectory, photo.PhotoName);

                DeletePhoto(thumbnailPath);
            }
        }

        private void DeletePhoto(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                throw new PhotoCleanupException(e);
            }
        }
    }
}