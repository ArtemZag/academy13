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

        public void CreateUser(UserModel user)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Create(user);
            }
        }

        public void UpdateUser(UserModel user)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Update(user);
            }
        }

        public void DeleteUser(UserModel user)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Delete(user);
            }
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