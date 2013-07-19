using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;
using System;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class UserService : Service, IUserService
    {
        public UserService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public bool RegisterUser(UserModel user)
        {
            try
            {
                using (var unitOfWork = this.WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Create(user);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool UpdateUser(UserModel user)
        {
            try
            {
                using (var unitOfWork = this.WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Update(user);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool DeleteUser(UserModel user)
        {
            try
            {
                using (var unitOfWork = this.WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Delete(user);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool CheckUser(UserModel user)
        {
            using (var unitOfWork = this.WorkFactory.GetUnitOfWork())
            {
                var userRepository = unitOfWork.Users;

                return userRepository.Contains(model => model.Equals(user));
            }
        }
    }
}