using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class AlbumService : Service, IAlbumService
    {
        public AlbumService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public ICollection<AlbumModel> GetAlbums(string userEmail)
        {
            ICollection<AlbumModel> result;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                result = user.Albums;
            }

            return result;
        }

        public AlbumModel GetAlbum(string userEmail, string albumName)
        {
            AlbumModel result;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                result = GetAlbum(user, albumName, unitOfWork);
            }

            return result;
        }

        public void CreateAlbum(string userEmail, AlbumModel album)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                user.Albums.Add(album);

                unitOfWork.Users.Update(user);
            }
        }

        public void UpdateAlbum(AlbumModel album)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Albums.Update(album);
            }
        }

        public void DeleteAlbum(string userEmail, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                user.Albums.Remove(album);
            }
        }

        private AlbumModel GetAlbum(UserModel user, string albumName, IUnitOfWork unitOfWork)
        {
            AlbumModel result;

            try
            {
                result =
                    unitOfWork.Albums.Find(
                        model => model.UserModelID == user.ID && string.Equals(model.AlbumName, albumName));
            }
            catch (Exception e)
            {
                throw new AlbumNotFoundException(e);
            }

            return result;
        }
    }
}