using System.Linq;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

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
            if (!IsUserExist(user.Email))
            {
                // todo: it will be parameter
                const string AUTH_PROVIDER = AuthInfoModel.LOCAL_PROFILE;

                // todo: local password will be in UserModel
                AuthInfoModel authInfoModel =
                    user.AuthInfos.First(model => string.Equals(model.AuthProvider, AUTH_PROVIDER));

                authInfoModel.UserPassword = cryptoProvider.CreateHashForPassword(authInfoModel.UserPassword,
                                                                                  cryptoProvider.Solt);

                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Add(user);
                    unitOfWork.SaveChanges();
                }
            }
            else
            {
                throw new UserAlreadyExistException(user.Email);
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

                    unitOfWork.SaveChanges();
                }
                catch (UserNotFoundException)
                {
                }
            }
        }

        public bool IsUserValid(string userEmail, string userPassword)
        {
            bool result;

            try
            {
                // todo: it will be parameter
                const string AUTH_PROVIDER = AuthInfoModel.LOCAL_PROFILE;

                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    UserModel user = GetUser(userEmail, unitOfWork);

                    // todo: local password will be in UserModel
                    AuthInfoModel authInfoModel =
                        user.AuthInfos.First(model => string.Equals(model.AuthProvider, AUTH_PROVIDER));

                    string dbPassword = authInfoModel.UserPassword;

                    result = cryptoProvider.IsPasswordsEqual(userPassword, dbPassword, cryptoProvider.Solt);
                }
            }
            catch (UserNotFoundException)
            {
                result = false;
            }

            return result;
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