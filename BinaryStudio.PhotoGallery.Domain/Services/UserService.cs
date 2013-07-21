using System;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

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
            catch
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
            catch
            {
                return false;
            }

            return true;
        }

        public bool DeleteUser(UserModel user)
        {
            try
            {
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Users.Delete(user);
                    unitOfWork.SaveChanges();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool CheckUser(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IUserRepository userRepository = unitOfWork.Users;

                return userRepository.Contains(model => string.Equals(model.Email, userEmail));
            }
        }
    }
}