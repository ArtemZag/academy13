using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class UserService : DbService, IUserService
    {
        private readonly ICryptoProvider _cryptoProvider;

        public UserService(IUnitOfWorkFactory workFactory, ICryptoProvider cryptoProvider)
            : base(workFactory)
        {
            _cryptoProvider = cryptoProvider;
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return
                    unitOfWork.Users.All()
                              .Include(g => g.Albums)
                              .Include(g => g.Groups)
                              .Include(g => g.AuthInfos)
                              .ToList();
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

        public UserModel GetUnactivatedUser(string hash)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel foundUser = unitOfWork.Users.Find(user => !user.IsActivated && user.Salt == hash);

                if (foundUser == null)
                {
                    throw new UserNotFoundException("Inactive user with hash '" + hash + "' not found");
                }

                return foundUser;
            }
        }

        public IEnumerable<UserModel> GetUnactivatedUsers()
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
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


        //todo: maybe we will remove this method?
        public void CreateUser(UserModel user, AuthInfoModel.ProviderType provider)
        {
            if (IsUserExist(user.Email))
            {
                throw new UserAlreadyExistException(user.Email);
            }

            if (provider == AuthInfoModel.ProviderType.Local)
            {
                user.Salt = _cryptoProvider.GetNewSalt();
                user.UserPassword = _cryptoProvider.CreateHashForPassword(user.UserPassword, user.Salt);
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

            var userModel = new UserModel
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
                        _cryptoProvider.CreateHashForPassword(Randomizer.GetString(16), _cryptoProvider.GetNewSalt())
                };

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Add(userModel);
                unitOfWork.SaveChanges();

                userModel.Albums = new Collection<AlbumModel>
                    {
                        new AlbumModel
                            {
                                AlbumName = "Temporary",
                                Description = "System album. Not for use",
                                OwnerId = unitOfWork.Users.Find(user => user.Email == userEmail).Id
                            }
                    };

                unitOfWork.Users.Update(userModel);
                unitOfWork.SaveChanges();
            }

            return userModel.Salt;
        }

        public void ActivateUser(string userEmail, string userPassword /*, string hash*/)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    UserModel userModel = GetUser(userEmail, unitOfWork);

                    if (userModel.IsActivated /* || (userModel.Salt != hash)*/)
                        throw new UserNotFoundException(userEmail);

                    userModel.Salt = _cryptoProvider.GetNewSalt();

                    userModel.UserPassword = _cryptoProvider.CreateHashForPassword(userPassword, userModel.Salt);
                    userModel.IsActivated = true;


                    unitOfWork.Users.Update(userModel);
                    unitOfWork.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
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
                catch (Exception)
                {
                    throw;
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

                    result = _cryptoProvider.IsPasswordsEqual(userPassword, user.UserPassword, user.Salt);
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

        public void MakeUserGod(int godID, int slaveID)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel god = GetUser(godID, unitOfWork);
                UserModel slave = GetUser(slaveID, unitOfWork);

                if (!god.IsAdmin)
                {
                    throw new RepentSlave(god.FirstName);
                }

                god.IsAdmin = false;
                slave.IsAdmin = true;
            }
        }
    }
}