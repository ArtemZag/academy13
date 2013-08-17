using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAllUsers();

        UserModel GetUser(int userId);

        UserModel GetUser(string userEmail);

        /// <summary>
        /// Finds not activated user by hash-code
        /// </summary>
        /// <param name="hash">Hash-code in Salt-field</param>
        /// <returns>User model</returns>
        UserModel GetUnactivatedUser(string hash);

        /// <summary>
        /// Finds all not activated users by
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserModel> GetUnactivatedUsers();

        int GetUserId(string userEmail);

        void CreateUser(UserModel userModel, AuthInfoModel.ProviderType providerType = AuthInfoModel.ProviderType.Local);


        /// <summary>
        /// Creates base user record in database for future activating it
        /// </summary>
        /// <param name="userEmail">Email for activating</param>
        /// <param name="userFirstName">First name of user</param>
        /// <param name="userLastName">Last name of user</param>
        /// <returns>128-symbols hash-code for activating link</returns>
        string CreateUser(string userEmail, string userFirstName, string userLastName);

        /// <summary>
        /// Activates user
        /// </summary>
        /// <param name="userEmail">User's Email</param>
        /// <param name="userPassword">New user's password</param>
        /// <param name="hash">Hash-code for activation</param>
        void ActivateUser(string userEmail, string userPassword/*, string hash*/);

        void DeleteUser(string userEmail);

        bool IsUserValid(string userEmail, string userPassword);

        bool IsUserExist(string userEmail);

        bool IsUserExist(string authProvider, string token);

        /// <summary>
        /// Makes user a God
        /// </summary>
        /// <param name="godID">userID with God permissions</param>
        /// <param name="slaveID"></param>
        void MakeUserGod(int godID, int slaveID);
    }
}