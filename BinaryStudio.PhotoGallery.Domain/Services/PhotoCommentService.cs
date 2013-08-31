using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoCommentService: DbService, IPhotoCommentService
    {
        private readonly ISecureService secureService;
        private readonly IGlobalEventsAggregator eventsAggregator;

        public PhotoCommentService(IUnitOfWorkFactory workFactory, ISecureService secureService, IGlobalEventsAggregator eventsAggregator) : base(workFactory)
        {
            this.secureService = secureService;
            this.eventsAggregator = eventsAggregator;
        }

        public IEnumerable<PhotoCommentModel> GetPhotoComments(int userId, int photoId, int begin, int last)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var albumId = unitOfWork.Photos.Find(photoId).AlbumModelId;

                if (secureService.CanUserViewComments(userId, albumId))
                {
                    return unitOfWork.PhotoComments.Filter(model => model.PhotoId == photoId)
                        .OrderBy(model => model.DateOfCreating)
                        .ThenBy(model => model.Id)
                        .Skip(begin).Take(last - begin)
                        .ToList();
                }
                throw new NoEnoughPrivilegesException("User can't get access to comments");
            }
        }

        public void AddPhotoComment(int userId, PhotoCommentModel newPhotoCommentModel)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var albumId = unitOfWork.Photos.Find(newPhotoCommentModel.PhotoId).AlbumModelId;

                if (secureService.CanUserAddComment(userId, albumId))
                {
                    unitOfWork.PhotoComments.Add(newPhotoCommentModel);
                    unitOfWork.SaveChanges();
                    eventsAggregator.PushCommentAddedEvent(newPhotoCommentModel);
                }
                else
                {
                    throw new NoEnoughPrivilegesException("User can't get access to comments");
                }
            }
        }

        public PhotoCommentModel GetPhotoComment(int commentId)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.PhotoComments.Find(commentId);
            }
        }

    }
}
