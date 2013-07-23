using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Core.UserUtils;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class UserService : Service, IUserService
    {
        private readonly ICryptoProvider cryptoProvider;

        public UserService(IUnitOfWorkFactory workFactory, ICryptoProvider cryptoProvider) : base(workFactory)
        {
            this.cryptoProvider = cryptoProvider;
        }

        public UserModel GetUser(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return GetUser(userEmail, unitOfWork);
            }
        }

        public void CreateUser(UserModel user)
        {
            // TODO user password come in opned view - you must make hash and then create user
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Add(user);
            }
        }

        public void UpdateUser(UserModel user)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Update(user);
            }
        }

        public void DeleteUser(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    UserModel user = GetUser(userEmail, unitOfWork);
                    unitOfWork.Users.Delete(user);
                }
                catch (UserNotFoundException)
                {
                }
            }
        }

        public bool IsUserValid(string userEmail, string userPassword)
        {
            // TODO this method must compare passwords too
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IUserRepository userRepository = unitOfWork.Users;

                return userRepository.Contains(model => string.Equals(model.Email, userEmail));
            }
        }

        public bool IsUserExist(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IUserRepository userRepository = unitOfWork.Users;

                return userRepository.Contains(model => string.Equals(model.Email, userEmail));
            }
        }
    }
}