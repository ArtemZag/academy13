using System;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;
using System.Linq;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal abstract class DbService
    {
        protected readonly IUnitOfWorkFactory WorkFactory;

        protected DbService(IUnitOfWorkFactory workFactory)
        {
            WorkFactory = workFactory;
        }

        protected UserModel GetUser(string userEmail, IUnitOfWork unitOfWork)
        {
            UserModel user = unitOfWork.Users.Find(model => string.Equals(model.Email, userEmail));

            if (user == null)
            {
                throw new UserNotFoundException(userEmail);
            }

            return user;
        }

        protected UserModel GetUser(int userId, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Users.Find(model => model.Id == userId);
        }

        protected AlbumModel GetAlbum(UserModel user, string albumName, IUnitOfWork unitOfWork)
        {
            try
            {
                return
                    user.Albums.Select(model => model).First(model => string.Equals(model.AlbumName, albumName) && !model.IsDeleted);
            }
            catch
            {
                throw new AlbumNotFoundException();
            }
        }

        protected AlbumModel GetAlbum(int albumId, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Albums.Find(model => model.Id == albumId && !model.IsDeleted);
        }
    }
}