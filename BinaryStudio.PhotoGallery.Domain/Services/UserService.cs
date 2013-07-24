﻿using BinaryStudio.PhotoGallery.Core.UserUtils;
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

        public void CreateUser(UserModel user, string provider)
        {
            if (IsUserExist(user.Email))
            {
                throw new UserAlreadyExistException(user.Email);
            }
            
            if (provider == AuthInfoModel.LOCAL_PROFILE)
            {
                user.Salt = this.cryptoProvider.Salt;
                user.UserPassword = this.cryptoProvider.CreateHashForPassword(user.UserPassword, user.Salt);
            }

            using (IUnitOfWork unitOfWork = this.WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Add(user);
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

        public bool IsUserValid(string userEmail, string enteredUserPassword)
        {
            bool result;

            try
            {
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    UserModel user = GetUser(userEmail, unitOfWork);

                    result = cryptoProvider.IsPasswordsEqual(enteredUserPassword, user.UserPassword, user.Salt);
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