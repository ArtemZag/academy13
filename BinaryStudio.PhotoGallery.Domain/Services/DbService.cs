using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal abstract class DbService
    {
        protected readonly IUnitOfWorkFactory WorkFactory;

        protected DbService(IUnitOfWorkFactory workFactory)
        {
            WorkFactory = workFactory;
        }

        protected AlbumModel GetAlbum(int albumId, IUnitOfWork unitOfWork)
        {
            AlbumModel foundAlbum = unitOfWork.Albums.Find(albumId);

            if (foundAlbum == null)
            {
                throw new AlbumNotFoundException(string.Format("Album with id {0} not found", albumId));
            }

            return foundAlbum;
        }

        protected UserModel GetUser(string userEmail, IUnitOfWork unitOfWork)
        {
            UserModel foundUser = unitOfWork.Users.Find(model => string.Equals(model.Email, userEmail));

            if (foundUser == null)
            {
                throw new UserNotFoundException(string.Format("User with email {0} not found", userEmail));
            }

            return foundUser;
        }

        protected UserModel GetUser(int userId, IUnitOfWork unitOfWork)
        {
            UserModel foundUser = unitOfWork.Users.Find(userId);

            if (foundUser == null)
            {
                throw new UserNotFoundException(string.Format("User with id {0} not found", userId));
            }

            return foundUser;
        }

        protected AlbumModel GetAlbum(int userId, int albumId, IUnitOfWork unitOfWork)
        {
            UserModel foundUser = unitOfWork.Users.Find(userId);

            if (foundUser == null)
            {
                throw new UserNotFoundException(string.Format("User with id {0} not found", userId));
            }

            try
            {
                return foundUser.Albums.First(album => album.Id == albumId && !album.IsDeleted);
            }
            catch
            {
                //todo: normal return message exception
                throw new AlbumNotFoundException(string.Format("Album with id {0} not found", albumId));
            }
        }

        protected AlbumModel GetAlbum(UserModel user, int albumId)
        {
            try
            {
                return
                    user.Albums.Select(model => model)
                        .First(model => model.Id == albumId && !model.IsDeleted);
            }
            catch
            {
                //todo: normal return message exception
                throw new AlbumNotFoundException(string.Format("Album with id {0} not found", albumId));
            }
        }

        protected AlbumModel GetAlbum(int userId, int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return this.GetAlbum(userId, albumId, unitOfWork);
            }
        }

        protected GroupModel GetGroup(int groupId, IUnitOfWork unitOfWork)
        {
            GroupModel groupModel = unitOfWork.Groups.Find(groupId);

            if (groupModel == null)
            {
                throw new GroupNotFoundException(string.Format("Group with ID {0} not found", groupId));
            }

            return groupModel;
        }

        protected GroupModel GetGroup(string groupName, IUnitOfWork unitOfWork)
        {
            GroupModel groupModel = unitOfWork.Groups.Find(group => group.GroupName == groupName);

            if (groupModel == null)
            {
                throw new GroupNotFoundException(string.Format("Group with name {0} not found", groupName));
            }

            return groupModel;
        }
    }
}