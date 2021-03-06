﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
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

        private const bool ADD_USER = true;
        private const bool REMOVE_USER = false;

        private readonly IAlbumService _albumService;
        private readonly ICryptoProvider _cryptoProvider;

        public UserService(IUnitOfWorkFactory workFactory, ICryptoProvider cryptoProvider, IAlbumService albumService)
            : base(workFactory)
        {
            _cryptoProvider = cryptoProvider;
            _albumService = albumService;
        }

        public IEnumerable<UserModel> GetAllUsers(int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var group = unitOfWork.Groups.Find(x => x.GroupName == "DeletedUsers");
                var users =
                    unitOfWork.Users
                        .All()
                        .Include(user => user.Albums)
                        .Include(user => user.Groups)
                        .Include(user => user.AuthInfos)
                        .OrderBy(user => user.DateOfCreating)
                        .ThenBy(user => user.Id)
                        .Skip(skipCount)
                        .Take(takeCount)
                        .ToList();

                return users.Where(user => !user.IsAdmin && !user.Groups.Contains(group));
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
                    throw new UserNotFoundException("Inactive user with invite '" + hash + "' not found");
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

        public void Update(UserModel userModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Update(userModel);
                unitOfWork.SaveChanges();
            }
        }

        public string CreateUser(string userEmail, string userFirstName, string userLastName)
        {
            if (IsUserExist(userEmail))
            {
                var userModel = GetUser(userEmail);
                // Here is our HASH for activating link
                userModel.Salt = Randomizer.GetString(16);

                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Update(userModel);
                    unitOfWork.SaveChanges();
                }

                return userModel.Salt;
            }
            else
            {
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

                    int userId = unitOfWork.Users.Find(user => user.Email == userEmail).Id;
                    _albumService.CreateSystemAlbums(userId);
                }

                return userModel.Salt;
            }
        }

        public void ActivateUser(string userEmail, string userPassword, string invite)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel userModel = GetUser(userEmail, unitOfWork);

                if (userModel.IsActivated)
                {
                    throw new UserAlreadyExistException(string.Format("User {0} already activated", userEmail));
                }

                if (userModel.Salt != invite)
                {
                    throw new UserNotFoundException(string.Format("User with email {0} not found in activation list",
                        userEmail));
                }

                userModel.Salt = _cryptoProvider.GetNewSalt();

                userModel.UserPassword = _cryptoProvider.CreateHashForPassword(userPassword, userModel.Salt);
                userModel.IsActivated = true;


                unitOfWork.Users.Update(userModel);
                unitOfWork.SaveChanges();
            }
        }

        public string UserRestorePasswordAsk(UserModel mUser)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                mUser.RemindPasswordSalt = Randomizer.GetString(30);

                unitOfWork.Users.Update(mUser);
                unitOfWork.SaveChanges();

                return mUser.RemindPasswordSalt;
            }
        }

        public void UserRestorePasswordChangePass(string userEmail, string userPassword)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel mUser = GetUser(userEmail, unitOfWork);
                mUser.Salt = _cryptoProvider.GetNewSalt();

                mUser.UserPassword = _cryptoProvider.CreateHashForPassword(userPassword, mUser.Salt);
                mUser.RemindPasswordSalt = null;

                unitOfWork.Users.Update(mUser);
                unitOfWork.SaveChanges();
            }
        }

        public void DeleteUser(int userId)
        {
            Expression<Func<GroupModel, bool>> expression = x => x.GroupName == "DeletedUsers";
            UserActionForGroup(userId, expression, ADD_USER);
        }

        public void BlockUser(int userId)
        {
            Expression<Func<GroupModel, bool>> expression = x => x.GroupName == "BlockedUsers";
            UserActionForGroup(userId, expression, ADD_USER);
        }

        public void UnblockUser(int userId)
        {
            Expression<Func<GroupModel, bool>> expression = x => x.GroupName == "BlockedUsers";
            UserActionForGroup(userId, expression, REMOVE_USER);
        }

        /// <summary>
        /// Adds or Removes user from group
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="expression">group specific</param>
        /// <param name="add">add [ADD_USER] or remove [REMOVE_USER] from group</param>
        private void UserActionForGroup(int userId, Expression<Func<GroupModel, bool>> expression, bool add)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    UserModel user = GetUser(userId, unitOfWork);

                    GroupModel group = unitOfWork.Groups.Find(expression);

                    if (add)
                    {
                        user.Groups.Add(@group);
                    }
                    else
                    {
                        user.Groups.Remove(@group);
                    }

                    unitOfWork.Users.Update(user);

                    unitOfWork.SaveChanges();
                }
                catch (UserNotFoundException)
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

                return userRepository.Contains(model => model.Email == userEmail);
            }
        }

        public bool IsUserExist(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IUserRepository userRepository = unitOfWork.Users;

                return userRepository.Contains(model => model.Id == userId && model.IsActivated);
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
                            string.Equals(model.AuthProvider, authProvider) &&
                            string.Equals(model.AuthProviderId, token));
            }
        }

        public void MakeUserGod(int godId, int slaveId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel god = GetUser(godId, unitOfWork);
                UserModel slave = GetUser(slaveId, unitOfWork);

                if (!god.IsAdmin)
                {
                    throw new RepentSlave(god.FirstName);
                }

                god.IsAdmin = false;
                slave.IsAdmin = true;
            }
        }

        public bool IsUserBlocked(int userId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork
                    .Users.Find(userId).Groups.ToList()
                    .Find(group => group.GroupName == "BlockedUsers") != null;
            }
        }

        public int GetUserBySocialAccount(string providerName, string id)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AuthInfoModel auth = unitOfWork.AuthInfos.Find(authInfo => authInfo.AuthProviderId == id &&
                                                                           authInfo.AuthProvider == providerName);
                if (auth == null)
                {
                    throw new UserNotFoundException(string.Format("There is some misstake in {0} authentication",
                        providerName));
                }
                return auth.UserId;
            }
        }

        public void UpdateUserAuthInfo(int userId, AuthInfoModel authInfoModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    UserModel user = GetUser(userId);
                    if (user.AuthInfos == null)
                    {
                        user.AuthInfos = new Collection<AuthInfoModel>();
                    }
                    else
                    {
                        user.AuthInfos.Add(authInfoModel);
                    }
                    unitOfWork.Users.Update(user);
                    unitOfWork.SaveChanges();
                }
                catch (Exception)
                {
                    throw new AuthenticationException(
                        string.Format("AuthInfoModel for user with id={0} does not available", userId));
                }
            }
        }

        public AuthInfoModel GetAuthInfoForUser(int userId, string providerName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    return
                        unitOfWork.AuthInfos.Find(
                            auth => auth.UserId == userId && auth.AuthProvider == providerName);
                }
                catch (Exception)
                {
                    throw new AuthenticationException(
                        string.Format("AuthInfoModel for user with id={0} does not available", userId));
                }
            }
        }

        public void AddAuthInfoForUser(int userId, string providerName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    unitOfWork.AuthInfos.Add(userId, providerName);
                    unitOfWork.SaveChanges();
                }
                catch (Exception)
                {
                    throw new AuthenticationException(
                        string.Format("AuthInfoModel for user with id={0} does not available", userId));
                }
            }
        }

        public void SetAuthInfoForUser(int userId, string providerName, string providerId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    unitOfWork.AuthInfos.Add(userId, providerName, providerId);
                    unitOfWork.SaveChanges();
                }
                catch (Exception)
                {
                    throw new AuthenticationException(
                        string.Format("AuthInfoModel for user with id={0} does not available", userId));
                }
            }
        }
    }
}