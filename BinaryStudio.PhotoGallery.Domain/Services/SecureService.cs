using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class SecureService : DbService, ISecureService
    {
        public SecureService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public bool CanUserViewComments(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeeComments;

            return CanUserDoCommentsAction(userId, albumId, predicate);
        }

        public bool CanUserAddComment(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanAddComments;

            return CanUserDoCommentsAction(userId, albumId, predicate);
        }

        public bool CanUserViewPhotos(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeePhotos;

            return CanUserDoCommentsAction(userId, albumId, predicate);
        }

        public bool CanUserAddPhoto(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanAddPhotos;

            return CanUserDoCommentsAction(userId, albumId, predicate);
        }

        public bool CanUserDeletePhoto(int userId, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = unitOfWork.Users.Find(userId);
                PhotoModel photo = unitOfWork.Photos.Find(photoId);

                // if user in admin group OR user is photo owner
                return (user.Groups.ToList().Find(model => (model.ID == 1)) != null) || (photo.UserId == userId);
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork)
        {
            UserModel user = GetUser(userId, unitOfWork);
            List<GroupModel> userGroups = user.Groups.ToList();

            return unitOfWork.Albums.Filter(album => album.AvailableGroups
                .ToList()
                .Find(group => userGroups
                    .Find(ag => ag.ID == group.Id) != null) != null).ToList();
        }

        /// <summary>
        ///     Checks if user take a part in even one group, that have enough permissions to do some action
        /// </summary>
        private bool CanUserDoCommentsAction(int userId, int albumId, Predicate<AvailableGroupModel> predicate)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                List<AvailableGroupModel> availableGropusCanDo =
                    unitOfWork.Albums.Find(albumId).AvailableGroups.ToList().FindAll(predicate);


                GroupModel userGroups = unitOfWork.Users.Find(userId)
                    .Groups
                    .ToList()
                    .Find(group => availableGropusCanDo.Find(x => x.GroupId == @group.ID) != null);

                return userGroups != null;
            }
        }
    }
}