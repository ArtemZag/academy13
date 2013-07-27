using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoCleaupTask : DbService, IPhotoCleanupTask
    {
        public PhotoCleaupTask(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public void Execute()
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                List<PhotoModel> photosToCleanup = unitOfWork.Photos.Filter(model => model.IsDeleted).ToList();

                photosToCleanup.ForEach(CleanPhoto);
            }
        }

        private void CleanPhoto(PhotoModel photoModel)
        {

        }
    }
}