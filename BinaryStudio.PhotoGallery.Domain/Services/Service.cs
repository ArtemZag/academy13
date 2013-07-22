using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal abstract class Service
    {
        protected readonly IUnitOfWorkFactory WorkFactory;

        protected Service(IUnitOfWorkFactory workFactory)
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
    }
}
