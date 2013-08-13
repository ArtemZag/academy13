using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class SecureService : ISecureService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public SecureService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
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
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.GetUnitOfWork())
            {
                UserModel user = unitOfWork.Users.Find(userId);
                PhotoModel photo = unitOfWork.Photos.Find(photoId);

                // if user in admin group OR user is photo owner
                return (user.Groups.ToList().Find(model => (model.ID == 1)) != null) || (photo.UserId == userId);
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork)
        {
            List<GroupModel> listAg = unitOfWork.Users.Find(userId).Groups.ToList();

            return unitOfWork.Albums.Filter(album => album.AvailableGroups
                .ToList()
                .Find(group => listAg
                    .Find(ag => ag.ID == group.ID) != null) != null).ToList();
        }

        /// <summary>
        ///     Checks if user take a part in even one group, that have enough permissions to do some action
        /// </summary>
        private bool CanUserDoCommentsAction(int userId, int albumId, Predicate<AvailableGroupModel> predicate)
        {
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.GetUnitOfWork())
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