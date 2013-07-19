using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        void RegisterUser(UserModel userModel);

        void UpdateUser(UserModel userModel);

        bool CheckUser(UserModel userModel);
    }
}