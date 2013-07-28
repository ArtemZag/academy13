using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoCleanupTask : DbService, IPhotoCleanupTask
    {
        private readonly IPhotoService photoService;

        public PhotoCleanupTask(IUnitOfWorkFactory workFactory, IPhotoService photoService) : base(workFactory)
        {
            this.photoService = photoService;
        }

        public void Execute()
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
        }

        private void CleanOriginal(PhotoModel photo)
        {
            string path = photoService.GetOriginalPhotoPath(photo);

            DeletePhoto(path);
        }

        private void CleanThumbnails(PhotoModel photo)
        {
            string thumbnailsDirectoryPath = photoService.GetThumbnailsPath(photo);
            IEnumerable<string> thumbnailsFormats = Directory.GetDirectories(thumbnailsDirectoryPath);

            foreach (var thumbnailsFormatDirectory in thumbnailsFormats)
            {
                string thumbnailPath = Path.Combine(thumbnailsFormatDirectory, photo.PhotoName);

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