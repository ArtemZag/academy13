using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoCleaupTask : DbService, IPhotoCleanupTask
    {
        private const string THUMBNAIL_DIRECTORY_NAME = "thumbnail";

        public PhotoCleaupTask(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
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

        private void CleanPhoto(PhotoModel photoModel)
        {
            CleanOriginal(photoModel);
            CleanThumbnail(photoModel);
        }

        private void CleanOriginal(PhotoModel photoModel)
        {
            // todo: what if album not exist in db
            // then I can't to get album name
            throw new NotImplementedException();
        }

        private void CleanThumbnail(PhotoModel photoModel)
        {
            throw new NotImplementedException();
        }
    }
}