using System;
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

        public AlbumModel GetAlbum(string userEmail, string albumName)
        {
            AlbumModel result;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

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
                unitOfWork.SaveChanges();
            }
        }

        public void UpdateAlbum(AlbumModel album)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Albums.Update(album);
                unitOfWork.SaveChanges();
            }
        }

        public void DeleteAlbum(AlbumModel album)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Albums.Delete(album);
                unitOfWork.SaveChanges();
            }
        }
    }
}