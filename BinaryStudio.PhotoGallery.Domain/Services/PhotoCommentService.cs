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
        public PhotoCommentService(IUnitOfWorkFactory workFactory, ISecureService secureService) : base(workFactory)
        {
            _secureService = secureService;
        }

        public IEnumerable<PhotoCommentModel> GetPhotoComments(int userID, int photoID, int begin, int last)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var albumID = unitOfWork.Photos.Find(photoID).AlbumModelId;

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
                var albumID = unitOfWork.Photos.Find(newPhotoCommentModel.PhotoModelId).AlbumModelId;

                if (_secureService.CanUserAddComment(userID, albumID))
                {
                    unitOfWork.PhotoComments.Add(newPhotoCommentModel);
                    unitOfWork.SaveChanges();
                }
                else
                {
                    throw new NoEnoughPrivileges("User can't get access to comments", null);
                }
                
            }
        }
    }
}
