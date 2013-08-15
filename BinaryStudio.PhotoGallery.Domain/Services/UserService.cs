using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;
using System.Linq;
using System.Data.Entity;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class UserService : DbService, IUserService
    {
        private readonly ICryptoProvider cryptoProvider;

        public UserService(IUnitOfWorkFactory workFactory, ICryptoProvider cryptoProvider) : base(workFactory)
        {
            this.cryptoProvider = cryptoProvider;
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
				return unitOfWork.Users.All().Include(g => g.Albums).Include(g => g.Groups).Include(g => g.AuthInfos).ToList();
            }
        }

        public UserModel GetUser(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Users.Find(user => user.Id == userId);
            }
        }

        public UserModel GetUser(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
	            return GetUser(userEmail, unitOfWork);
            }
        }

        public UserModel FindNonActivatedUser(string hash)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
	            return unitOfWork.Users.Find(user => !user.IsActivated && user.Salt == hash);
            }
        }

        public IEnumerable<UserModel> FindNonActivatedUsers()
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Users.Filter(user => !user.IsActivated);
            }
        }

        public int GetUserId(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Users.Find(user => user.Email == userEmail).Id;
            }
        }

        public void CreateUser(UserModel user, AuthInfoModel.ProviderType provider)
        {
            if (IsUserExist(user.Email))
            {
                throw new UserAlreadyExistException(user.Email);
            }

            if (provider == AuthInfoModel.ProviderType.Local)
            {
                user.Salt = cryptoProvider.GetNewSalt();
                user.UserPassword = cryptoProvider.CreateHashForPassword(user.UserPassword, user.Salt);
            }

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Add(user);
                unitOfWork.SaveChanges();
            }
        }


        public string CreateUser(string userEmail, string userFirstName, string userLastName)
        {
            if (IsUserExist(userEmail))
            {
                throw new UserAlreadyExistException(userEmail);
            }


            var userModel = new UserModel()
                {
                    Email = userEmail,
                    FirstName = userFirstName,
                    LastName = userLastName,
                    IsAdmin = false,
                    IsActivated = false,
                    // Here is our HASH for activating link
                    Salt = Randomizer.GetString(16),
                    // Empty password field is not good 
                    UserPassword =
                        cryptoProvider.CreateHashForPassword(Randomizer.GetString(16), cryptoProvider.GetNewSalt())
                    
                };

            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Add(userModel);
                unitOfWork.SaveChanges();
            }

            return userModel.Salt;
        }

        public void ActivateUser(string userEmail, string userPassword, string hash)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
				var userModel = this.GetUser(userEmail);

				if (userModel.IsActivated || (userModel.Salt != hash)) return;

				unitOfWork.Users.Find(userModel.Id).Salt = cryptoProvider.GetNewSalt();

				unitOfWork.Users.Find(userModel.Id).UserPassword = cryptoProvider.CreateHashForPassword(userPassword, userModel.Salt);
				unitOfWork.Users.Find(userModel.Id).IsActivated = true;
				unitOfWork.SaveChanges();
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
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    UserModel user = GetUser(userEmail, unitOfWork);

                    result = cryptoProvider.IsPasswordsEqual(userPassword, user.UserPassword, user.Salt);
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

        /// <summary>
        ///     Checks if there is a user with given token
        /// </summary>
        /// <param name="authProvider">[facebook][google]</param>
        /// <param name="token">Token for authorization</param>
        public bool IsUserExist(string authProvider, string token)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IAuthInfoRepository authInfoRepository = unitOfWork.AuthInfos;

                return
                    authInfoRepository.Contains(
                        model =>
                        string.Equals(model.AuthProvider, authProvider) && string.Equals(model.AuthProviderToken, token));
            }
        }
    }
}