﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    class SecureService : ISecureService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public SecureService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }




        public bool CanUserViewComments(int userID, int albumID)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeeComments;

            return CanUserDoCommentsAction(userID, albumID, predicate);
        }

        public bool CanUserAddComment(int userID, int albumID)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanAddComments;

            return CanUserDoCommentsAction(userID, albumID, predicate);
        }

        public bool CanUserViewPhotos(int userID, int albumID)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeePhotos;

            return CanUserDoCommentsAction(userID, albumID, predicate);
        }

        public bool CanUserAddPhoto(int userID, int albumID)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanAddPhotos;

            return CanUserDoCommentsAction(userID, albumID, predicate);
        }

        public bool CanUserDeletePhoto(int userID, int photoID)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {

                var user = unitOfWork.Users.Find(userID);
                var photo = unitOfWork.Photos.Find(photoID);

                // if user in admin group OR user is photo owner
                return (user.Groups.ToList().Find(model => (model.ID == 1)) != null) || (photo.UserModelId == userID);
            }
        }


        /// <summary>
        /// Checks if user take a part in even one group, that have enough permissions to do some action
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="albumID"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private bool CanUserDoCommentsAction(int userID, int albumID, Predicate<AvailableGroupModel> predicate)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var availableGropusCanDo = unitOfWork.Albums.Find(albumID).AvailableGroups.ToList().FindAll(predicate);


                var userGroups = unitOfWork.Users.Find(userID)
                                           .Groups
                                           .ToList()
                                           .Find(group => availableGropusCanDo.Find(x => x.GroupModelId == @group.ID) != null);

                return userGroups != null;
            }
        }
    }
}