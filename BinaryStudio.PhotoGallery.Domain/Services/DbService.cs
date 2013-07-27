﻿using BinaryStudio.PhotoGallery.Database;
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

        protected UserModel GetUser(string userEmail, IUnitOfWork unitOfWork)
        {
            UserModel user = unitOfWork.Users.Find(model => string.Equals(model.Email, userEmail));

            if (user == null)
            {
                throw new UserNotFoundException(userEmail);
            }

            return user;
        }

        protected AlbumModel GetAlbum(UserModel user, string albumName, IUnitOfWork unitOfWork)
        {
            AlbumModel album =
                unitOfWork.Albums.Find(
                    model => model.UserModelId == user.Id && string.Equals(model.AlbumName, albumName));

            if (album == null)
            {
                throw new AlbumNotFoundException();
            }

            return album;
        }
    }
}