using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        void CreateUser(UserModel userModel);

        void UpdateUser(UserModel userModel);

        void DeleteUser(UserModel userModel);

        bool CheckUser(string userEmail);
    }
}