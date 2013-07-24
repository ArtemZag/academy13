using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        UserModel GetUser(string userEmail);

        void CreateUser(UserModel userModel, string provider = AuthInfoModel.LOCAL_PROFILE);

        void DeleteUser(string userEmail);

        bool IsUserValid(string userEmail, string enteredUserPassword);

        bool IsUserExist(string userEmail);
    }
}