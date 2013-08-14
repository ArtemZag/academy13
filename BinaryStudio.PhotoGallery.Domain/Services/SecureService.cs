using System;
using System.Collections.Generic;
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
                return (user.Groups.ToList().Find(model => (model.Id == 1)) != null) || (photo.UserId == userId);
            }
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork)
        {
            UserModel user = GetUser(userId, unitOfWork);
            List<GroupModel> userGroups = user.Groups.ToList();

            IEnumerable<int> albumIds = unitOfWork.AvailableGroups.All().ToList().Join(userGroups,
                avialableGroupModel => avialableGroupModel.Id,
                groupModel => groupModel.Id,
                (avialableGroupModel, groupModel) => new {avialableGroupModel.CanSeePhotos, avialableGroupModel.AlbumId})
                .Where(arg => arg.CanSeePhotos).Select(arg => arg.AlbumId).Distinct();


            return albumIds.Select(albumId => GetAlbum(albumId, unitOfWork)).ToList();
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
                    .Find(group => availableGropusCanDo.Find(x => x.GroupId == @group.Id) != null);

                return userGroups != null;
            }
        }
    }
}