using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoCommentService: DbService, IPhotoCommentService
    {
        public PhotoCommentService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public IEnumerable<PhotoCommentModel> GetPhotoComments(int photoID, int begin, int last)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var result = unitOfWork.PhotoComments.Filter(model => model.PhotoModelId == photoID)
                              .OrderBy(model => model.DateOfCreating)
                              .ThenBy(model => model.Id)
                              .Skip(begin).Take(last - begin);

                // Maaak: here is fix for lazy loading of data, 
                //        when DbContext is already disposed(using block), but result is not generated yet
                return result.ToList();
            }
        }

        public void AddPhotoComment(PhotoCommentModel newPhotoCommentModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.PhotoComments.Add(newPhotoCommentModel);
                unitOfWork.SaveChanges();
            }
        }
    }
}
