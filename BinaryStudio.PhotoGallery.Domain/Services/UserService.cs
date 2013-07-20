using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;
using System;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class UserService : Service, IUserService
    {

        // changed by AT: use class for constructor, not an interface
        // public UserService(IUnitOfWorkFactory workFactory) : base(workFactory)
        public UserService(UnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public bool CreateUser(UserModel user)
        {
            try
            {
                using (var unitOfWork = this.WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Create(user);
                    unitOfWork.SaveChanges();
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
                    unitOfWork.SaveChanges();
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
                    unitOfWork.SaveChanges();
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