using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class SecureService : DbService, ISecureService
    {
        public SecureService(IUnitOfWorkFactory workFactory)
            : base(workFactory)
        {
        }

        public bool CanUserViewComments(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeeComments;

            return CanUserDoAction(userId, albumId, predicate);
        }

        public bool CanUserAddComment(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanAddComments;

            return CanUserDoAction(userId, albumId, predicate);
        }

        public bool CanUserViewPhotos(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeePhotos;

            return CanUserDoAction(userId, albumId, predicate);
        }

        public bool CanUserAddPhoto(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanAddPhotos;

            return CanUserDoAction(userId, albumId, predicate);
        }

        public bool CanUserDeletePhoto(int userId, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = unitOfWork.Users.Find(userId);
                PhotoModel photo = unitOfWork.Photos.Find(photoId);
                AlbumModel album = unitOfWork.Albums.Find(photo.AlbumId);

                // if user is album owner OR user is photo owner OR user is admin
                return (album.OwnerId == userId) || (photo.OwnerId == userId) || (user.IsAdmin);
            }
        }

        public bool CanUserViewLikes(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeeLikes;

            return CanUserDoAction(userId, albumId, predicate);
        }

        /// <summary>
        ///     Returns all public albums except users albums
        /// </summary>
        public IEnumerable<AlbumModel> GetPublicAlbums(int userId, IUnitOfWork unitOfWork)
        {
            var result = new List<AlbumModel>();

            UserModel user = GetUser(userId, unitOfWork);

            if (user.IsAdmin)
            {
                result.AddRange(unitOfWork.Albums.All());
            }
            else
            {
                List<int> userAlbumsIds = user.Albums.Select(model => model.Id).ToList();

                List<GroupModel> userGroups = user.Groups.ToList();

                IEnumerable<int> albumIds = unitOfWork.AvailableGroups.All().ToList().Join(userGroups,
                    avialableGroupModel => avialableGroupModel.GroupId, groupModel => groupModel.Id,
                    (avialableGroupModel, groupModel) => new
                    {
                        avialableGroupModel.CanSeePhotos,
                        avialableGroupModel.AlbumId
                    })
                    .Where(arg => arg.CanSeePhotos)
                    .Select(arg => arg.AlbumId)
                    .Distinct();

                result.AddRange(
                    albumIds.Where(id => !userAlbumsIds.Contains(id))
                    .Select(albumId => GetAlbum(albumId, unitOfWork))
                    .Where(model => !model.IsDeleted));
            }

            return result;
        }


        public void LetGroupViewComments(int userId, int groupId, int albumId, bool let)
        {
            //todo: add try-catch
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AvailableGroupModel availableGroupView = GetAvailableGroup(userId, groupId, albumId, unitOfWork);

                availableGroupView.CanSeeComments = let;

                unitOfWork.AvailableGroups.Update(availableGroupView);
                unitOfWork.SaveChanges();
            }
        }


        public void LetGroupAddComment(int userId, int groupId, int albumId, bool let)
        {
            //todo: add try-catch
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AvailableGroupModel availableGroupView = GetAvailableGroup(userId, groupId, albumId, unitOfWork);

                availableGroupView.CanAddComments = let;

                unitOfWork.AvailableGroups.Update(availableGroupView);
                unitOfWork.SaveChanges();
            }
        }

        public void LetGroupViewPhotos(int userId, int groupId, int albumId, bool let)
        {
            //todo: add try-catch
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AvailableGroupModel availableGroupView = GetAvailableGroup(userId, groupId, albumId, unitOfWork);

                availableGroupView.CanSeePhotos = let;

                unitOfWork.AvailableGroups.Update(availableGroupView);
                unitOfWork.SaveChanges();
            }
        }

        public void LetGroupAddPhoto(int userId, int groupId, int albumId, bool let)
        {
            //todo: add try-catch
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AvailableGroupModel availableGroupView = GetAvailableGroup(userId, groupId, albumId, unitOfWork);

                availableGroupView.CanAddPhotos = let;

                unitOfWork.AvailableGroups.Update(availableGroupView);
                unitOfWork.SaveChanges();
            }
        }

        public void LetGroupViewLikes(int userId, int groupId, int albumId, bool let)
        {
            //todo: add try-catch
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AvailableGroupModel availableGroupView = GetAvailableGroup(userId, groupId, albumId, unitOfWork);

                availableGroupView.CanSeeLikes = let;

                unitOfWork.AvailableGroups.Update(availableGroupView);
                unitOfWork.SaveChanges();
            }
        }


        /// <summary>
        ///     Checks if user take a part in even one group, that have enough permissions to do some action
        /// </summary>
        private bool CanUserDoAction(int userId, int albumId, Predicate<AvailableGroupModel> predicate)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                bool canUser;

                UserModel user = GetUser(userId, unitOfWork);
                AlbumModel album = GetAlbum(albumId, unitOfWork);


                if (user.IsAdmin || (album.OwnerId == userId))
                {
                    canUser = true;
                }
                else
                {
                    List<AvailableGroupModel> availableGropusCanDo =
                        unitOfWork.Albums.Find(albumId).AvailableGroups.ToList().FindAll(predicate);


                    GroupModel userGroups = unitOfWork.Users.Find(userId).Groups.ToList()
                        .Find(
                            group =>
                                availableGropusCanDo.Find(x => x.GroupId == @group.Id) != null);

                    canUser = userGroups != null;
                }

                return canUser;
            }
        }

        /// <summary>
        ///     Gets available group or creates if doesn't exist.
        /// </summary>
        private AvailableGroupModel GetAvailableGroup(int userId, int groupId, int albumId, IUnitOfWork unitOfWork)
        {
            AlbumModel album = GetAlbum(albumId, unitOfWork);
            UserModel user = GetUser(userId, unitOfWork);

            if ((album.OwnerId != userId) && !user.IsAdmin)
                throw new UserHaveNoEnoughPrivilegesException(
                    string.Format("User (id={0}) can't let or deny group (id={1}) view comments in album (id={2})",
                        userId, groupId, albumId));

            return album.AvailableGroups.ToList().Find(ag => ag.GroupId == groupId) ??
                   new AvailableGroupModel
                   {
                       AlbumId = albumId,
                       GroupId = groupId,
                   };
        }
    }
}