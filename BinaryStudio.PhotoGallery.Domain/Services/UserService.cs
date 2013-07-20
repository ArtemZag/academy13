using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;
using System;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class UserService : Service, IUserService
    {
        public UserService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public bool CreateUser(UserModel user)
        {
            try
            {
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Create(user);
                    unitOfWork.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool UpdateUser(UserModel user)
        {
            try
            {
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Update(user);
                    unitOfWork.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool DeleteUser(UserModel user)
        {
            try
            {
                using (IUnitOfWork unitOfWork = this.WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Delete(user);
                    unitOfWork.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool CheckUser(UserModel user)
        {
            using (IUnitOfWork unitOfWork = this.WorkFactory.GetUnitOfWork())
            {
                IUserRepository userRepository = unitOfWork.Users;

                return userRepository.Contains(model => model.Equals(user));
            }
        }
    }
}