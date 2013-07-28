using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        UserModel GetUser(string userEmail);

        void CreateUser(UserModel userModel, AuthInfoModel.ProviderType providerType = AuthInfoModel.ProviderType.Local);

        void DeleteUser(string userEmail);

        bool IsUserValid(string userEmail, string userPassword);

        bool IsUserExist(string userEmail);
    }
}