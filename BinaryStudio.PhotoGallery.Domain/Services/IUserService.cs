using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAllUsers(int skipCount, int takeCount);

        UserModel GetUser(int userId);

        UserModel GetUser(string userEmail);

        /// <summary>
        ///     Finds not activated user by invite-code
        /// </summary>
        /// <param name="hash">Hash-code in Salt-field</param>
        /// <returns>User model</returns>
        UserModel GetUnactivatedUser(string hash);

        /// <summary>
        ///     Finds all not activated users by
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserModel> GetUnactivatedUsers();

        int GetUserId(string userEmail);

        void Update(UserModel user);

        /// <summary>
        ///     Creates base user record in database for future activating it
        /// </summary>
        /// <param name="userEmail">Email for activating</param>
        /// <param name="userFirstName">First name of user</param>
        /// <param name="userLastName">Last name of user</param>
        /// <returns>128-symbols invite-code for activating link</returns>
        string CreateUser(string userEmail, string userFirstName, string userLastName);

        /// <summary>
        ///     Activates user
        /// </summary>
        /// <param name="userEmail">User's Email</param>
        /// <param name="userPassword">New user's password</param>
        /// <param name="invite">Hash-code for activation</param>
        void ActivateUser(string userEmail, string userPassword, string invite);

        void DeleteUser(int userId);

        bool IsUserValid(string userEmail, string userPassword);

        bool IsUserExist(int userId);

        bool IsUserExist(string userEmail);

        bool IsUserExist(string authProvider, string token);

        /// <summary>
        ///     Makes user a God
        /// </summary>
        /// <param name="godId">userID with God permissions</param>
        /// <param name="slaveId"></param>
        void MakeUserGod(int godId, int slaveId);

        bool IsUserBlocked(int userId);

        /// <summary>
        /// Checks if user have social network account and it is valid for him
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetUserBySocialAccount(string providerName, string id);
    }
}