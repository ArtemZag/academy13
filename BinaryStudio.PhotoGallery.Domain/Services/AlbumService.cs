using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;
using System.Linq;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class AlbumService : DbService, IAlbumService
    {
        public AlbumService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public void CreateAlbum(string userEmail, AlbumModel album)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                user.Albums.Add(album);

                unitOfWork.SaveChanges();
            }
        }

        public void DeleteAlbum(string userEmail, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                album.IsDeleted = true;

                unitOfWork.SaveChanges();
            }
        }

        public IEnumerable<AlbumModel> GetAlbums(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                return user.Albums.Select(model => model).Where(model => !model.IsDeleted);
            }
        }

        public AlbumModel GetAlbum(string userEmail, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                return GetAlbum(user, albumName, unitOfWork);
            }
        }
    }
}