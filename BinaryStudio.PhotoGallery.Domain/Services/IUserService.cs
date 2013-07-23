using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        UserModel GetUser(string userEmail);

        void CreateUser(UserModel userModel);

        /// <summary>
        /// Updates info about user.
        /// </summary>
        /// <param name="userModel">Must contains full info about user.</param>
        void UpdateUser(UserModel userModel);

        void DeleteUser(string userEmail);

        bool IsUserValid(string userEmail, string userPassword);

        bool IsUserExist(string userEmail);
    }
}