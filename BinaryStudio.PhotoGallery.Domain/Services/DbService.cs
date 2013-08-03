using System.Linq;
using System.Runtime.CompilerServices;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

[assembly: InternalsVisibleTo("BinaryStudio.PhotoGallery.Domain.Tests")]

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
            try
            {
                return
                    user.Albums.Select(model => model)
                        .First(model => string.Equals(model.AlbumName, albumName) && !model.IsDeleted);
            }
            catch
            {
                throw new AlbumNotFoundException();
            }
        }
    }
}