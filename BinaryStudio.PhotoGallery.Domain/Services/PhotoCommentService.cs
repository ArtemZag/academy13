using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoCommentService: DbService, IPhotoCommentService
    {
        private readonly ISecureService _secureService;
        private readonly IGlobalEventsAggregator _eventsAggregator;

        public PhotoCommentService(IUnitOfWorkFactory workFactory, ISecureService secureService, IGlobalEventsAggregator eventsAggregator) : base(workFactory)
        {
            _secureService = secureService;
            _eventsAggregator = eventsAggregator;
        }

        public IEnumerable<PhotoCommentModel> GetPhotoComments(int userID, int photoID, int begin, int last)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var albumID = unitOfWork.Photos.Find(photoID).AlbumId;

                if (_secureService.CanUserViewComments(userID, albumID))
                {
                    return unitOfWork.PhotoComments.Filter(model => model.PhotoModelId == photoID)
                                     .OrderBy(model => model.DateOfCreating)
                                     .ThenBy(model => model.Id)
                                     .Skip(begin).Take(last - begin)
                                     .ToList();
                }

                throw new NoEnoughPrivileges("User can't get access to comments", null);
            }
        }

        public void AddPhotoComment(int userID, PhotoCommentModel newPhotoCommentModel)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var albumID = unitOfWork.Photos.Find(newPhotoCommentModel.PhotoModelId).AlbumId;

                if (_secureService.CanUserAddComment(userID, albumID))
                {
                    unitOfWork.PhotoComments.Add(newPhotoCommentModel);
                    unitOfWork.SaveChanges();
                    _eventsAggregator.PushCommentAddedEvent(newPhotoCommentModel);
                }
                else
                {
                    throw new NoEnoughPrivileges("User can't get access to comments", null);
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
